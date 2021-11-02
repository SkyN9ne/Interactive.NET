﻿// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.DotNet.Interactive.Formatting;
using Microsoft.DotNet.Interactive.Formatting.TabularData;

namespace Microsoft.DotNet.Interactive
{
    [TypeFormatterSource(typeof(DataExplorerFormatterSource))]
    public abstract class DataExplorer<TData>
    {
        public string Id { get; } = Guid.NewGuid().ToString("N");

        public TData Data { get; }

        protected DataExplorer(TData data)
        {
            Data = data;
        }

        public static void RegisterFormatters()
        {
            Formatter.Register<DataExplorer<TData>>((explorer, writer) =>
            {        
                explorer.ToHtml().WriteTo(writer, HtmlEncoder.Default);
            }, HtmlFormatter.MimeType);
            
            Formatter.SetPreferredMimeTypesFor(typeof(DataExplorer<TData>), HtmlFormatter.MimeType, TabularDataResourceFormatter.MimeType);
        }

        static DataExplorer()
        {
            RegisterFormatters();
        }

        protected abstract IHtmlContent ToHtml();

        public static void Register<TDataExplorer>() where TDataExplorer : DataExplorer<TData>
        {
            DataExplorer.Register(typeof(TData), typeof(TDataExplorer));
        }
    }
}