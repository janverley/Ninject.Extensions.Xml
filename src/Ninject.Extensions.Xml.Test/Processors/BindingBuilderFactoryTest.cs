//-------------------------------------------------------------------------------
// <copyright file="BindingBuilderFactoryTest.cs" company="Ninject Project Contributors">
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
    using System;
    using System.Xml;
    using System.Xml.Linq;
    using FluentAssertions;
    using Moq;
    using Ninject.Extensions.Xml.Fakes;
    using Ninject.Planning.Bindings;
    using Ninject.Syntax;
    using Xunit;

    public class BindingBuilderFactoryTest
    {
        private BindingBuilderFactory testee;

        public BindingBuilderFactoryTest()
        {
            this.testee = new BindingBuilderFactory();
        }

        [Fact]
        public void CreateBindingWithTo()
        {
            var bindToSyntaxMock = new Mock<IBindingToSyntax<object>>();
            var bindingRootMock = CreateBindingRootMock(bindToSyntaxMock);
            var bindingSyntax = new Mock<IBindingConfigurationSyntax<object>>().Object;
            var bindingElement = CreateBindToXmlElement(
                "Ninject.Extensions.Xml.Fakes.IWeapon, Ninject.Extensions.Xml.Test",
                "Ninject.Extensions.Xml.Fakes.Sword, Ninject.Extensions.Xml.Test");

            bindToSyntaxMock.Setup(br => br.To(typeof(Sword))).Returns(bindingSyntax);

            var result = this.testee.Create(bindingElement, bindingRootMock.Object);

            result.Should().BeSameAs(bindingSyntax);
        }

        [Fact]
        public void CreateBindingWithToProvider()
        {
            var bindToSyntaxMock = new Mock<IBindingToSyntax<object>>();
            var bindingRootMock = CreateBindingRootMock(bindToSyntaxMock);
            var bindingSyntax = new Mock<IBindingConfigurationSyntax<object>>().Object;
            var bindingElement = CreateBindToProviderXmlElement(
                "Ninject.Extensions.Xml.Fakes.IWeapon, Ninject.Extensions.Xml.Test",
                "Ninject.Extensions.Xml.Fakes.Sword, Ninject.Extensions.Xml.Test");

            bindToSyntaxMock.Setup(br => br.ToProvider(typeof(Sword))).Returns(bindingSyntax);

            var result = this.testee.Create(bindingElement, bindingRootMock.Object);

            result.Should().BeSameAs(bindingSyntax);
        }

        [Fact]
        public void CreateThrowsExceptionIfNoServiceIsFound()
        {
            var bindingRootMock = CreateBindingRootMock(new Mock<IBindingToSyntax<object>>());
            var bindingElement = new XElement("bind");

            Action createAction = () => this.testee.Create(bindingElement, bindingRootMock.Object);

            createAction.ShouldThrow<XmlException>()
                .WithMessage("The 'bind' element does not have the required attribute 'service'.");
        }

        [Fact]
        public void CreateThrowsExceptionIfServiceTypeCanNotBeResolved()
        {
            var bindingRootMock = CreateBindingRootMock(new Mock<IBindingToSyntax<object>>());
            var bindingElement = CreateBindXmlElement("UnknownType, Ninject.Extensions.Xml.Test");

            Action createAction = () => this.testee.Create(bindingElement, bindingRootMock.Object);

            createAction.ShouldThrow<XmlException>()
                .WithMessage("Couldn't resolve type 'UnknownType, Ninject.Extensions.Xml.Test' defined in 'service' attribute.");
        }
        
        [Fact]
        public void CreateThrowsExceptionIfNoTargetIsFound()
        {
            var bindingRootMock = CreateBindingRootMock(new Mock<IBindingToSyntax<object>>());
            var bindingElement = CreateBindXmlElement("Ninject.Extensions.Xml.Fakes.IWeapon, Ninject.Extensions.Xml.Test");

            Action createAction = () => this.testee.Create(bindingElement, bindingRootMock.Object);

            createAction.ShouldThrow<XmlException>()
                .WithMessage("The 'bind' element does not define either a 'to' or 'toProvider' attribute.");
        }

        [Fact]
        public void CreateThrowsExceptionIfToAndToProviderIsDefined()
        {
            var bindingRootMock = CreateBindingRootMock(new Mock<IBindingToSyntax<object>>());
            var bindingElement = CreateBindXmlElement(
                "Ninject.Extensions.Xml.Fakes.IWeapon, Ninject.Extensions.Xml.Test",
               new XAttribute("to", "to"),
               new XAttribute("toProvider", "provider"));

            Action createAction = () => this.testee.Create(bindingElement, bindingRootMock.Object);

            createAction.ShouldThrow<XmlException>()
                .WithMessage("The 'bind' element has both a 'to' and a 'toProvider' attribute. Specify only one of them!");
        }

        [Fact]
        public void CreateThrowsExceptionIfToTypeCanNotBeResolved()
        {
            var bindingRootMock = CreateBindingRootMock(new Mock<IBindingToSyntax<object>>());
            var bindingElement = CreateBindToXmlElement(
                "Ninject.Extensions.Xml.Fakes.IWeapon, Ninject.Extensions.Xml.Test",
                "UnknownType, Ninject.Extensions.Xml.Test");

            Action createAction = () => this.testee.Create(bindingElement, bindingRootMock.Object);

            createAction.ShouldThrow<XmlException>()
                .WithMessage("Couldn't resolve type 'UnknownType, Ninject.Extensions.Xml.Test' defined in 'to' attribute.");
        }

        [Fact]
        public void CreateThrowsExceptionIfToProviderTypeCanNotBeResolved()
        {
            var bindingRootMock = CreateBindingRootMock(new Mock<IBindingToSyntax<object>>());
            var bindingElement = CreateBindToProviderXmlElement(
                "Ninject.Extensions.Xml.Fakes.IWeapon, Ninject.Extensions.Xml.Test",
                "UnknownType, Ninject.Extensions.Xml.Test");

            Action createAction = () => this.testee.Create(bindingElement, bindingRootMock.Object);

            createAction.ShouldThrow<XmlException>()
                .WithMessage("Couldn't resolve type 'UnknownType, Ninject.Extensions.Xml.Test' defined in 'toProvider' attribute.");
        }

        private static Mock<IBindingRoot> CreateBindingRootMock(Mock<IBindingToSyntax<object>> bindToSyntaxMock)
        {
            var bindingRootMock = new Mock<IBindingRoot>();
            bindingRootMock.Setup(br => br.Bind(typeof(IWeapon))).Returns(bindToSyntaxMock.Object);
            return bindingRootMock;
        }

        private static XElement CreateBindToXmlElement(string service, string implementation)
        {
            var toAttribute = new XAttribute("to", implementation);
            XElement bindingElement = CreateBindXmlElement(service, toAttribute);
            return bindingElement;
        }

        private static XElement CreateBindToProviderXmlElement(string service, string implementation)
        {
            var toAttribute = new XAttribute("toProvider", implementation);
            XElement bindingElement = CreateBindXmlElement(service, toAttribute);
            return bindingElement;
        }
        
        private static XElement CreateBindXmlElement(string service, params XAttribute[] targetAttributes)
        {
            var serviceAttribute = new XAttribute("service", service);
            var bindingElement = new XElement("bind");
            bindingElement.Add(serviceAttribute);
            bindingElement.Add(targetAttributes);
            return bindingElement;
        }
    }
}
#endif