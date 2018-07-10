﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SparqlForHumans.Core.Utilities
{
    class SharpZipHandler
    {
        public static IEnumerable<string> ReadGZip(string filename)
        {
            var fileToDecompress = new FileInfo(filename);
            if (!fileToDecompress.Exists)
                yield break;

            using (var originalFileStream = fileToDecompress.OpenRead())
            using (var zip = new ICSharpCode.SharpZipLib.GZip.GZipInputStream(originalFileStream))
            using (var unzip = new StreamReader(zip))
            {
                while (!unzip.EndOfStream)
                    yield return unzip.ReadLine();
            }
        }
    }
}