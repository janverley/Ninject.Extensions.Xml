//-------------------------------------------------------------------------------
// <copyright file="ConstructorArgumentXmlElementProcessorTest.cs" company="Ninject Project Contributors">
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

#if !NO_GENERIC_MOQ && !NO_MOQ
namespace Ninject.Extensions.Xml.Processors
{
    using System.Xml;
    using System.Xml.Linq;

    using FluentAssertions;

    using Xunit;

    public class ConstructorArgumentXmlElementProcessorTest : ProcessorTestsBase<ConstructorArgumentXmlElementProcessor>
    {
        public ConstructorArgumentXmlElementProcessorTest()
            : base(Tags.HasConstructorArgument, "constructorArgument")
        {
        }

        [Fact]
        public void NameAndValueAttributeAreUsedToSetMetadataOnBinding()
        {
            const string Name = "TheName";
            const string Value = "TheValue";

            var element = new XElement("constructorArgument");
            element.Add(new XAttribute("name", Name));
            element.Add(new XAttribute("value", Value));

            var syntaxMock = CreateBindingSyntaxMock();
            var testee = this.CreateTestee();
            
            testee.Process(element, CreateOwner(), syntaxMock.Object);

            syntaxMock.Verify(s => s.WithConstructorArgument(Name, Value));
        }

        [Fact]
        public void NameAttributeIsRequired()
        {
            var element = new XElement("constructorArgument");
            element.Add(new XAttribute("value", "Value"));

            var syntaxMock = CreateBindingSyntaxMock();
            var testee = this.CreateTestee();

            var exception = Assert.Throws<XmlException>(() => testee.Process(element, CreateOwner(), syntaxMock.Object));

            exception.Message.Should().Be("The 'constructorArgument' element does not have the required attribute 'name'.");
        }

        [Fact]
        public void ValueAttributeIsRequired()
        {
            var element = new XElement("constructorArgument");
            element.Add(new XAttribute("name", "Name"));

            var syntaxMock = CreateBindingSyntaxMock();
            var testee = this.CreateTestee();

            var exception = Assert.Throws<XmlException>(() => testee.Process(element, CreateOwner(), syntaxMock.Object));

            exception.Message.Should().Be("The 'constructorArgument' element does not have the required attribute 'value'.");
        }

        protected override ConstructorArgumentXmlElementProcessor CreateTestee()
        {
            return new ConstructorArgumentXmlElementProcessor();
        }
    }
}
#endif