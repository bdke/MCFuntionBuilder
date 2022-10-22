using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCFBuilder.Type;
using MCFBuilder.Type.Compiler;

namespace MCFBuilder.Utility
{

    public struct ProgramLines
    {
        public string? FilePath { get; set; }
        public List<string?> Lines { get; set; }
    }

    public static class FunctionCompiler
    {
        public static ProgramLines Lines = new();
    }

    public class Scoreboard
    {
        public ScoreboardValues ScoreboardValues;

        public Scoreboard(ScoreboardValues scoreboardValues)
        {
            this.ScoreboardValues = scoreboardValues;
            if (scoreboardValues.File != ScoreboardValues.RootPath)
            {
                FunctionCompiler.Lines
                    .FilePath = scoreboardValues.File;
                FunctionCompiler.Lines
                    .Lines
                    .Add(
                        new($"{CommandAttribute.Compile()}scoreboard objectives add " +
                        $"{scoreboardValues.Name} " +
                        $"{ScoreboardValues.GetScoreboardTypes(ScoreboardValues.ScoreboardType)}")
                    );
            }
        }

        public void Add(object? value, string? selector)
        {
            FunctionCompiler.Lines.Lines.Add($"{CommandAttribute.Compile()}scoreboard players add {selector} {ScoreboardValues.Name} {value}");
        }

        public void Add(string? name, string? selector, string? selector2)
        {
            FunctionCompiler.Lines.Lines.Add($"{CommandAttribute.Compile()}scoreboard players operation {selector} {ScoreboardValues.Name} += {selector2} {name}");
        }

        public void Subtract(object? value, string? selector)
        {
            FunctionCompiler.Lines.Lines.Add($"{CommandAttribute.Compile()}scoreboard players remove {selector} {ScoreboardValues.Name} {value}");
        }
        public void Subtract(string? name, string? selector, string? selector2)
        {
            FunctionCompiler.Lines.Lines.Add($"{CommandAttribute.Compile()}scoreboard players operation {selector} {ScoreboardValues.Name} -= {selector2} {name}");
        }

        public void Multiply(object? value, string? selector)
        {
            FunctionCompiler.Lines.Lines.Add($"{CommandAttribute.Compile()}scoreboard players set @s .number {value}");
            FunctionCompiler.Lines.Lines.Add($"{CommandAttribute.Compile()}scoreboard players operation {selector} {ScoreboardValues.Name} *= @s .number");
        }

        public void Multiply(string? name, string? selector, string? selector2)
        {
            FunctionCompiler.Lines.Lines.Add($"{CommandAttribute.Compile()}scoreboard players operation {selector} {ScoreboardValues.Name} *= {selector2} {name}");
        }

        public void Divide(object? value, string? selector)
        {
            FunctionCompiler.Lines.Lines.Add($"{CommandAttribute.Compile()}scoreboard players set @s .number {value}");
            FunctionCompiler.Lines.Lines.Add($"{CommandAttribute.Compile()}scoreboard players operation {selector} {ScoreboardValues.Name} /= @s .number");
        }

        public void Divide(string? name, string? selector, string? selector2)
        {
            FunctionCompiler.Lines.Lines.Add($"{CommandAttribute.Compile()}scoreboard players operation {selector} {ScoreboardValues.Name} /= {selector2} {name}");
        }

        public void Remainder(object? value, string? selector)
        {
            FunctionCompiler.Lines.Lines.Add($"{CommandAttribute.Compile()}scoreboard players set @s .number {value}");
            FunctionCompiler.Lines.Lines.Add($"{CommandAttribute.Compile()}scoreboard players operation {selector} {ScoreboardValues.Name} %= @s .number");
        }

        public void Remainder(string? name, string? selector, string? selector2)
        {
            FunctionCompiler.Lines.Lines.Add($"{CommandAttribute.Compile()}scoreboard players operation {selector} {ScoreboardValues.Name} %= {selector2} {name}");
        }

        public void Set(object? value, string? selector)
        {
            FunctionCompiler.Lines.Lines.Add($"{CommandAttribute.Compile()}scoreboard players set {selector} {ScoreboardValues.Name} {value}");
        }

        public void Set(string? name ,string? selector, string? selector2)
        {
            FunctionCompiler.Lines.Lines.Add($"{CommandAttribute.Compile()}scoreboard players operation {selector} {ScoreboardValues.Name} = {selector2} {name}");
        }

        //public void EqualID(object? value, string? selector, string? selector2)
        //{
        //    FunctionCompiler.Lines.Lines.Add($"scoreboard players operation {selector} {ScoreboardValues.Name} = {value} {selector2}");
        //}
    }

    public static class Tags
    {
        public static void Add(string? name, string? selector)
        {
            FunctionCompiler.Lines.Lines.Add($"{CommandAttribute.Compile()}tag {selector} add {name}");
        }

        public static void Remove(string? name, string? selector)
        {
            FunctionCompiler.Lines.Lines.Add($"{CommandAttribute.Compile()}tag {selector} remove {name}");
        }
    }
}
