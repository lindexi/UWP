﻿using System;
using System.IO;
using Lindexi.Src.GitCommand;
using Lindexi.Src.WhitmanRandomIdentifier;

namespace NelyercairjachayjearnemHenakawbujairfo
{
    class Program
    {
        static void Main(string[] args)
        {
            var git = new Git(new DirectoryInfo(Environment.CurrentDirectory));

            var randomIdentifier = new RandomIdentifier();
            randomIdentifier.WordCount = 2;
            var name = $"t/lindexi/{randomIdentifier.Generate(true)}";

            git.CheckoutNewBranch(name);
        }
    }
}
