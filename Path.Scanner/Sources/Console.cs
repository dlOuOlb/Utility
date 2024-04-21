using S = System;
using G = System.Collections.Generic;
using C = System.Console;

namespace Utility
{
	public class Console( S.Boolean error ): S.Object( )
	{
		private static G.Dictionary<S.Boolean, Value> Dictionarize<Value>( Value former, Value latter ) => new( capacity: +2 ) { { false, former }, { true, latter } };

		private static readonly S.Object mutex = new( );
		private static readonly G.IReadOnlyDictionary<S.Boolean, Console> consoles = Dictionarize( former: new Console( error: false ), latter: new Console( error: true ) );

		private readonly S.IO.TextWriter writer = error ? C.Error : C.Out;
		private readonly G.IReadOnlyDictionary<S.Boolean, S.ConsoleColor> colors = Dictionarize( former: error ? S.ConsoleColor.DarkRed : S.ConsoleColor.DarkGray, latter: error ? S.ConsoleColor.Red : S.ConsoleColor.Gray );

		public static void Print( S.Boolean error = false ) => consoles[ key: error ].Print( );
		public static void Print( S.String message, S.Boolean significant = false, S.Boolean error = false ) => consoles[ key: error ].Print( emphasize: significant, message: message );

		private void Print( ) => this.writer.WriteLine( );
		private void Print( S.Boolean emphasize, S.String message )
		{
			lock( mutex )
			{
				var color = C.ForegroundColor;

				C.ForegroundColor = this.colors[ key: emphasize ];

				try { this.writer.WriteLine( value: message ); return; }
				catch { throw; }
				finally { C.ForegroundColor = color; }
			}
		}
	}
}
