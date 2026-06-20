// SPDX-FileCopyrightText: 2026 WithLithum
// SPDX-License-Identifier: LGPL-3.0-or-later

using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using OpenReinforce.Engine.Data.Models;

namespace OpenReinforce.Engine.Data
{
    public class FrContainerDataLoader<T, TContainer>
        where T : class, IFrIdentified
        where TContainer : class, IFrDataRoot<T>, new()
    {
        private readonly XmlSerializer _serializer = new XmlSerializer(typeof(TContainer));

        public TContainer Load(string path, string id)
        {
            if (!Directory.Exists(path))
            {
                throw new DirectoryNotFoundException(path);
            }

            var list = new List<T>();

            // The primary file.
            var primaryFile = Path.Combine(path, $"{id}.xml");
            AppendContainer(list, primaryFile);

            // "custom" data files. If necessary, override ones with the specified script name.
            var customDir = Path.Combine(path, "custom");
            if (Directory.Exists(customDir))
            {
                foreach (var file in Directory.EnumerateFiles(customDir, $"{id}_*.xml"))
                {
                    AppendContainer(list, file);
                }
            }

            // Assemble the container.
            return new TContainer
            {
                Items = list.ToArray()
            };
        }

        private void AppendContainer(List<T> list, string file)
        {
            if (!File.Exists(file))
            {
                return;
            }

            TContainer? result;
            using (var stream = File.OpenRead(file))
            {
                try
                {
                    result = (TContainer)_serializer.Deserialize(stream);
                }
                catch (XmlException)
                {
                    return;
                }
            }

            if (result == null || result.Items == null)
            {
                return;
            }

            // Override existing script names when adding.
            foreach (var item in result.Items)
            {
                list.RemoveAll(existing => existing.ScriptName == item.ScriptName);
                list.Add(item);
            }
        }
    }
}
