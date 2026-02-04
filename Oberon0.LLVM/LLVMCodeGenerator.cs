#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using LLVMSharp.Interop;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Exceptions;
using Oberon0.Compiler.Generator;
using Oberon0.Compiler.Types;
using Oberon0.Generator.LLVM.GeneratorInfo;

namespace Oberon0.Generator.LLVM;

/// <summary>
/// LLVM Code Generator für Oberon0
/// </summary>
/// <remarks>
/// Initialisiert den LLVM-Generator
/// </remarks>
/// <param name="module">Das zu übersetzende Modul</param>
public partial class LLVMCodeGenerator(Module module) : ICodeGenerator
{
    private LLVMModuleRef _llvmModule;
    private LLVMBuilderRef _builder;
    private LLVMContextRef _context;
    private bool _isGenerated;

    /// <summary>
    /// Das Compiler-Modul
    /// </summary>
    public Module Module { get; } = module ?? throw new ArgumentNullException(nameof(module));

    /// <summary>
    /// Der Name der Hauptklasse (für Kompatibilität)
    /// </summary>
    public string MainClassName { get; set; } = module.Name + "__Impl";

    /// <summary>
    /// Der Namespace der Hauptklasse (für Kompatibilität)
    /// </summary>
    public string MainClassNamespace { get; set; } = "Oberon0." + module.Name;

    /// <summary>
    /// Gibt den generierten LLVM IR Code als String zurück
    /// </summary>
    /// <returns>LLVM IR Code</returns>
    public string IntermediateCode()
    {
        if (!_isGenerated)
        {
            throw new InternalCompilerException("GenerateIntermediateCode must be called before IntermediateCode");
        }

        return _llvmModule.PrintToString();
    }

    /// <summary>
    /// Schreibt den generierten Code in einen TextWriter
    /// </summary>
    /// <param name="writer">Der TextWriter</param>
    public void WriteIntermediateCode(TextWriter writer)
    {
        if (!_isGenerated)
        {
            throw new InternalCompilerException("GenerateIntermediateCode must be called before WriteIntermediateCode");
        }

        writer.Write(IntermediateCode());
    }

    /// <summary>
    /// Startet die Code-Generierung
    /// </summary>
    public void GenerateIntermediateCode()
    {
        if (Module == null)
        {
            throw new InternalCompilerException("Module must be set before calling GenerateIntermediateCode");
        }

        // LLVM Kontext und Module erstellen
        _context = LLVMContextRef.Global;
        _llvmModule = _context.CreateModuleWithName(Module.Name ?? "Oberon0Module");
        _builder = _context.CreateBuilder();

        try
        {
            // Module-Struktur generieren
            GenerateModule();

            // Verifiziere das generierte Modul
            if (!_llvmModule.TryVerify(LLVMVerifierFailureAction.LLVMPrintMessageAction, out string error))
            {
                throw new InternalCompilerException($"LLVM module verification failed: {error}");
            }

            _isGenerated = true;
        }
        catch (Exception ex)
        {
            throw new InternalCompilerException($"Error during LLVM code generation: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Generiert eine Binärdatei (Objekt-Datei oder ausführbare Datei)
    /// </summary>
    /// <param name="options">Optionen für die Binär-Generierung</param>
    /// <returns>true bei Erfolg</returns>
    public bool GenerateBinary(CreateBinaryOptionsBase? options = null)
    {
        if (!_isGenerated)
        {
            GenerateIntermediateCode();
        }

        var binaryCreator = new CreateLLVMBinary(this, options as LlvmCreateBinaryOptions);
        return binaryCreator.Execute();
    }

    private void GenerateModule()
    {
        // Globale Variablen generieren
        foreach (var variable in Module.Block.Declarations.OfType<VariableDeclaration>())
        {
            GenerateGlobalVariable(variable);
        }

        // Prozeduren und Funktionen generieren
        foreach (var procedure in Module.Block.Procedures)
        {
            GenerateProcedure(procedure);
        }

        // Main-Funktion generieren (wenn kein Export-Modul)
        if (!Module.HasExports)
        {
            GenerateMainFunction();
        }
    }

    private void GenerateGlobalVariable(VariableDeclaration variable)
    {
        var llvmType = GetLLVMType(variable.Type);
        var globalVar = _llvmModule.AddGlobal(llvmType, variable.Name);
        globalVar.Initializer = LLVMValueRef.CreateConstInt(llvmType, 0);
        globalVar.Linkage = LLVMLinkage.LLVMInternalLinkage;
    }

    private void GenerateProcedure(FunctionDeclaration procedure)
    {
        // Funktionstyp erstellen
        var returnType = GetLLVMType(procedure.ReturnType);

        var paramTypes = procedure.Block.Declarations.OfType<ProcedureParameterDeclaration>()
            .Select(p => GetLLVMType(p.Type))
            .ToArray();

        var functionType = LLVMTypeRef.CreateFunction(returnType, paramTypes);
        var function = _llvmModule.AddFunction(procedure.Name, functionType);

        // Entry-Block erstellen
        var entryBlock = _context.AppendBasicBlock(function, "entry");
        _builder.PositionAtEnd(entryBlock);

        // TODO: Prozedur-Body generieren
        // Hier würde die Statement-Verarbeitung stattfinden

        // Vorläufiger Return
        if (returnType.Kind == LLVMTypeKind.LLVMVoidTypeKind)
        {
            _builder.BuildRetVoid();
        }
        else
        {
            _builder.BuildRet(LLVMValueRef.CreateConstInt(returnType, 0));
        }
    }

    private void GenerateMainFunction()
    {
        var mainType = LLVMTypeRef.CreateFunction(LLVMTypeRef.Int32, []);
        var mainFunction = _llvmModule.AddFunction("main", mainType);

        var entryBlock = _context.AppendBasicBlock(mainFunction, "entry");
        _builder.PositionAtEnd(entryBlock);

        // TODO: Hauptprogramm-Block generieren
        // Hier würde Module.Block.Statements verarbeitet werden

        // Return 0
        _builder.BuildRet(LLVMValueRef.CreateConstInt(LLVMTypeRef.Int32, 0));
    }

    private static LLVMTypeRef GetLLVMType(TypeDefinition type) => type.Type switch
    {
        BaseTypes.Int => LLVMTypeRef.Int32,
        BaseTypes.Bool => LLVMTypeRef.Int1,
        BaseTypes.Real => LLVMTypeRef.Double,
        BaseTypes.Void => LLVMTypeRef.Void,
        _ => throw new InternalCompilerException($"Unsupported type: {type}")
    };

    /// <summary>
    /// Gibt Ressourcen frei
    /// </summary>
    public void Dispose()
    {
        if (_isGenerated)
        {
            _builder.Dispose();
            _llvmModule.Dispose();
        }
    }
}