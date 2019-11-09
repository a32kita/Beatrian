using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        Console.WriteLine("** Quick product name replacer **");

        // parameters
        var targetDir = "./Source";
        var extensions = new string[]
        {
            "cs", "csproj", "sln", "projitems", "shproj"
        };

        var search = "Wagtail";
        var replace = "Beatrian";

        // backup
        var now = DateTime.Now;
        var bkMaxRetry = 99;
        for (var i = 0; i < bkMaxRetry; i++)
        {
            var bkDestPath =
                targetDir + "-bak" + String.Format("{0:00}{1:00}{2:00}-{3:00}", now.Year % 100, now.Month, now.Day, i);
            if (Directory.Exists(bkDestPath))
            {
                if (i < bkMaxRetry)
                    continue;

                Console.WriteLine("[Backup Error]");
                Console.WriteLine("Directory is already exist;");
                Console.WriteLine(bkDestPath);

                Environment.Exit(1);
            }

            Console.WriteLine("Backup directory...");
            Console.WriteLine("FROM: {0}", targetDir);
            Console.WriteLine("DEST: {0}", bkDestPath);

            //Directory.Copy(targetDir, bkDestPath));
            //Microsoft.VisualBasic.FileIO.FileSystem.CopyDirectory(targetDir, bkDestPath);
            DirUtils.CopyDirectory(targetDir, bkDestPath);

            break;
        }
        // files
        var files = new List<string>();
        foreach (var ext in extensions)
        {
            // files.AddRange(Directory.GetFiles(targetDir, SearchOption.AllDirectories, "*." + ext.ToLower()));
            // files.AddRange(Directory.GetFiles(targetDir, SearchOption.AllDirectories, "*." + ext.ToUpper()));
            files.AddRange(Directory.GetFiles(targetDir, "*." + ext, SearchOption.AllDirectories));
        }

        var failureFiles = new ErrorCollection();
        foreach (var f in files)
        {
            try
            {
                Console.WriteLine("Processing: {0}", Path.GetFileName(f));

                // file text
                var source = File.ReadAllText(f);
                File.WriteAllText(f, source.Replace(search, replace));

                // file name
                var fdir = Path.GetDirectoryName(f);
                var fname = Path.GetFileNameWithoutExtension(f);
                var fext = Path.GetExtension(f);
                fname = fname.Replace(search, replace);
                File.Move(f, Path.Combine(fdir, fname + fext));
            }
            catch (Exception ex)
            {
                failureFiles.Add(f, ex.Message);
            }
        }

        Console.WriteLine("File Result: total={0}, failured={1}", files.Count, failureFiles.Count);

        // directory
        var dirs = new List<string>();
        dirs.AddRange(Directory.GetDirectories(targetDir, "*" + search + "*", SearchOption.AllDirectories));
        dirs.AddRange(Directory.GetDirectories(targetDir, search + "*", SearchOption.AllDirectories));
        dirs.AddRange(Directory.GetDirectories(targetDir, "*" + search, SearchOption.AllDirectories));
        var dirsSorted = dirs.OrderByDescending(s => s.Length);

        var failureDirectories = new ErrorCollection();
        foreach (var d in dirsSorted)
        {
            try
            {
                Console.WriteLine("Processing: {0}", Path.GetFileName(d));

                // Directory name
                var ddir = Path.GetDirectoryName(d);
                var dname = Path.GetFileName(d);
                dname = dname.Replace(search, replace);
                Directory.Move(d, Path.Combine(ddir, dname));
            }
            catch (Exception ex)
            {
                failureDirectories.Add(d, ex.Message);
            }
        }
        
        Console.WriteLine("Dir Result: total={0}, failured={1}", dirs.Count, failureDirectories.Count);


        if (failureFiles.Count + failureDirectories.Count == 0)
            Environment.Exit(0);
        
        // Error report
        if (failureFiles.Count > 0)
        {
            Console.WriteLine("[File processing error]");
            foreach (var fe in failureFiles)
                Console.WriteLine("Error: {0} / {1}", fe.Item, fe.Message);
        }

        if (failureDirectories.Count > 0)
        {
            Console.WriteLine("[Directory processing error]");
            foreach (var de in failureDirectories)
                Console.WriteLine("Error: {0} / {1}", de.Item, de.Message);
        }
    }
}

class ErrorCollection : List<ErrorInfo>
{
    public void Add(string item, string message)
    {
        this.Add(new ErrorInfo()
        {
            Item = item,
            Message = message,
        });
    }
}

class ErrorInfo
{
    public string Item
    {
        get;
        set;
    }

    public string Message
    {
        get;
        set;
    }
}

static class DirUtils
{
    // dobon.net 様より
    // https://dobon.net/vb/dotnet/file/copyfolder.html

    /*
     * The MIT License (MIT)
     * 
     * Copyright (c) 2016 DOBON! <http://dobon.net>
     * 
     * Permission is hereby granted, free of charge, to any person obtaining a copy of this
     * software and associated documentation files (the "Software"), to deal in the Software
     * without restriction, including without limitation the rights to use, copy, modify, merge,
     * publish, distribute, sublicense, and/or sell copies of the Software, and to permit
     * persons to whom the Software is furnished to do so, subject to the following conditions:
     * 
     * The above copyright notice and this permission notice shall be included in all copies or
     * substantial portions of the Software.
     * 
     * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
     * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
     * PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
     * FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
     * OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
     * DEALINGS IN THE SOFTWARE.
     */

    /// <summary>
    /// ディレクトリをコピーする
    /// </summary>
    /// <param name="sourceDirName">コピーするディレクトリ</param>
    /// <param name="destDirName">コピー先のディレクトリ</param>
    public static void CopyDirectory(
        string sourceDirName, string destDirName)
    {
        //コピー先のディレクトリがないときは作る
        if (!System.IO.Directory.Exists(destDirName))
        {
            System.IO.Directory.CreateDirectory(destDirName);
            //属性もコピー
            System.IO.File.SetAttributes(destDirName, 
                System.IO.File.GetAttributes(sourceDirName));
        }

        //コピー先のディレクトリ名の末尾に"\"をつける
        if (destDirName[destDirName.Length - 1] !=
                System.IO.Path.DirectorySeparatorChar)
            destDirName = destDirName + System.IO.Path.DirectorySeparatorChar;

        //コピー元のディレクトリにあるファイルをコピー
        string[] files = System.IO.Directory.GetFiles(sourceDirName);
        foreach (string file in files)
            System.IO.File.Copy(file,
                destDirName + System.IO.Path.GetFileName(file), true);

        //コピー元のディレクトリにあるディレクトリについて、再帰的に呼び出す
        string[] dirs = System.IO.Directory.GetDirectories(sourceDirName);
        foreach (string dir in dirs)
            CopyDirectory(dir, destDirName + System.IO.Path.GetFileName(dir));
    }
}