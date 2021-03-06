//-------------------------------------------------------------------------------
// <copyright file="ExtensionsForXElementTests.cs" company="Ninject Project Contributors">
//   Copyright (c) 2009-2011 Ninject Project Contributors
//   Authors: Remo Gloor (remo.gloor@gmail.com)
//           
//   Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
//   you may not use this file except in compliance with one of the Licenses.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//   or
//       http://www.microsoft.com/opensource/licenses.mspx
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
//-------------------------------------------------------------------------------

namespace Ninject.Extensions.Xml.Extensions
{
    using System.Xml;
    using System.Xml.Linq;

    using FluentAssertions;

    using Xunit;

    public class ExtensionsForXElementTests
    {
        private readonly XDocument document;

        private readonly XElement element;

        public ExtensionsForXElementTests()
        {
            this.document = XDocument.Load("Cases/basic.xml");
            this.element = this.document.Element("module");
        }

        [Fact]
        public void RequiredAttributeRetunsValueIfExists()
        {
            var name = this.element.RequiredAttribute("name");

            name.Name.LocalName.Should().Be("name");
            name.Value.Should().Be("basicTest");
        }
    
        [Fact]
        public void RequiredAttributeThrowsExceptionIfAttributeNotExists()
        {
            AssertionExtensions.ShouldThrow<XmlException>(() => this.element.RequiredAttribute("noneExistingAttribute"));
        }
    }
}