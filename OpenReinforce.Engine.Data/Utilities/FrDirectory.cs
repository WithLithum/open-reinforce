// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

using System;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace OpenReinforce.Engine.Data.Utilities
{
    public static class FrDirectory
    {
        public static T? ReadData<T>(string directory,
            string fileName,
            Func<XmlReader, T?> reader,
            Func<T, T, T> merger)
            where T : class
        {
            T? main = null;

            var mainFile = Path.Combine(directory, $"{fileName}.xml");
            var customDir = Path.Combine(directory, "custom");

            main = ReadDataInternalSingle(mainFile, reader);
            if (main == null)
            {
                return null;
            }

            if (!Directory.Exists(customDir))
            {
                return main;
            }

            foreach (var customFile in Directory.EnumerateFiles(customDir,
                $"{fileName}_*.xml"))
            {
                var data = ReadDataInternalSingle(customFile, reader);
                if (data != null)
                {
                    main = merger(main, data);
                }
            }

            return main;
        }

        private static T? ReadDataInternalSingle<T>(string fileName,
            Func<XmlReader, T?> reader)
            where T : class
        {
            try
            {
                using var stream = File.OpenRead(fileName);
                var xmlRead = XmlReader.Create(stream);
                return reader(xmlRead);
            }
            catch (IOException e)
            {
                Debug.WriteLine(e);
                return null;
            }
        }
    }
}
