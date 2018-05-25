﻿using SparqlForHumans.Core.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace SparqlForHumans.UnitTests
{
    public class FileHelperTests
    {
        [Fact]
        public void TestGetFileType()
        {
            Assert.Equal(FileHelper.FileType.Unkwown, FileHelper.GetFileType(".txt"));
            Assert.Equal(FileHelper.FileType.nTriples, FileHelper.GetFileType(".nt"));
            Assert.Equal(FileHelper.FileType.gZip, FileHelper.GetFileType(".gz"));
        }

        [Fact]
        public void TestGetFilenameType()
        {
            Assert.Equal(FileHelper.FileType.Unkwown, FileHelper.GetFilenameType("Resources/empty.txt"));
            Assert.Equal(FileHelper.FileType.nTriples, FileHelper.GetFilenameType("Resources/empty.nt"));
            Assert.Equal(FileHelper.FileType.gZip, FileHelper.GetFilenameType("Resources/empty.gz"));
            Assert.Equal(FileHelper.FileType.gZip, FileHelper.GetFilenameType("Resources/empty.nt.gz"));
        }

        [Fact]
        public void TestReadLines()
        {
            string filename = "Resources/TenLines.txt";

            Assert.True(File.Exists(filename));

            Assert.NotNull(FileHelper.ReadLines(filename));
            Assert.NotEmpty(FileHelper.ReadLines(filename));
            Assert.Equal(10, FileHelper.ReadLines(filename).Count());
        }

    }
}
