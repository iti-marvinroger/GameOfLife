using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using NUnit.Framework;

namespace ITI.GameOfLife.ModelChecker
{
    [TestFixture]
    public class PublicModelChecker
    {
        static readonly Assembly _implementationAssembly = typeof( Game ).Assembly;

        [Explicit]
        [Test]
        public void write_current_public_implementation_to_console_with_double_quotes()
        {
            Console.WriteLine( GetPublicAPI( _implementationAssembly ).ToString().Replace( "\"", "\"\"" ) );
        }

        [Test]
        public void the_implementation_must_only_expose_the_Game()
        {
            var model = XElement.Parse( @"
<Assembly Name=""ITI.GameOfLife"">
  <Types>
    <Type Name=""ITI.GameOfLife.Game"">
      <Member Type=""Constructor"" Name="".ctor"" />
      <Member Type=""Method"" Name=""Equals"" />
      <Member Type=""Method"" Name=""get_Height"" />
      <Member Type=""Method"" Name=""get_Width"" />
      <Member Type=""Method"" Name=""GetHashCode"" />
      <Member Type=""Method"" Name=""GetType"" />
      <Member Type=""Method"" Name=""GiveLive"" />
      <Member Type=""Property"" Name=""Height"" />
      <Member Type=""Method"" Name=""IsAlive"" />
      <Member Type=""Method"" Name=""Kill"" />
      <Member Type=""Method"" Name=""NextTurn"" />
      <Member Type=""Method"" Name=""ToString"" />
      <Member Type=""Property"" Name=""Width"" />
    </Type>
  </Types>
</Assembly>" );
            CheckPublicAPI( model, _implementationAssembly );
        }

        void CheckPublicAPI( XElement model, Assembly assembly )
        {
            XElement current = GetPublicAPI( assembly );
            if( !XElement.DeepEquals( model, current ) )
            {
                string m = model.ToString( SaveOptions.DisableFormatting );
                string c = current.ToString( SaveOptions.DisableFormatting );
                Assert.That( c, Is.EqualTo( m ) );
            }
        }

        XElement GetPublicAPI( Assembly a )
        {
            return new XElement( "Assembly",
                new XAttribute( "Name", a.GetName().Name ),
                new XElement( "Types",
                    AllNestedTypes( a.GetExportedTypes() )
                        .OrderBy( t => t.FullName )
                        .Select( t => new XElement( "Type",
                            new XAttribute( "Name", t.FullName ),
                            t.GetMembers()
                                .OrderBy( m => m.Name )
                                .Select( m => new XElement( "Member",
                                    new XAttribute( "Type", m.MemberType ),
                                    new XAttribute( "Name", m.Name ) ) ) ) ) ) );
        }

        IEnumerable<Type> AllNestedTypes( IEnumerable<Type> types )
        {
            foreach( Type t in types )
            {
                yield return t;
                foreach( Type nestedType in AllNestedTypes( t.GetNestedTypes() ) )
                {
                    yield return nestedType;
                }
            }
        }
    }
}
