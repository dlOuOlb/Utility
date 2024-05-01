using S = System;
using G = System.Collections.Generic;
using static System.Linq.Enumerable;

namespace Utility
{
	public static partial class Program: S.Object
	{
		private static readonly G.HashSet<S.String> extensions = [ @".dll", @".exe" ];

		private static S.Exception? Verify( S.IO.FileInfo argument, S.IO.FileInfo variable )
		{
			try { return File.Data.Equals( former: argument, latter: variable ) ? null : throw new S.Exception( message: @"Different file contents." ); }
			catch( S.Exception exception ) { return exception; }
			finally { }
		}

		private static void Verify( G.IEnumerable<S.String> arguments, G.IEnumerable<S.String> variables )
		{
			var directories = new { arguments = File.Name.Scan( directories: arguments ), variables = File.Name.Scan( directories: variables ) };

			foreach( var argument in directories.arguments )
				if( argument.Value is null )
					Console.Print( message: $"- \"{argument.Key}\"", significant: true, error: true );
				else
				{
					Console.Print( message: $"- \"{argument.Key}\"", significant: true );

					foreach( var variable in directories.variables )
						if( variable.Value is null )
							Console.Print( message: $"\t- \"{variable.Key}\"", error: true );
						else
						{
							var names = File.Name.Intersect( former: argument.Value, latter: variable.Value );

							Console.Print( message: $"\t- \"{variable.Key}\"", significant: names.Any( ) );

							foreach( var name in names )
							{
								var significant = extensions.Contains( item: S.IO.Path.GetExtension( path: name ).ToLowerInvariant( ) );
								var file = new
								{
									argument = new S.IO.FileInfo( fileName: S.IO.Path.Combine( path1: argument.Key, path2: name ) ),
									variable = new S.IO.FileInfo( fileName: S.IO.Path.Combine( path1: variable.Key, path2: name ) )
								};

								if( Verify( argument: file.argument, variable: file.variable ) is S.Exception exception )
									Console.Print( message: $"\t\t- \"{name}\": {exception.Message}", significant: significant, error: true );
								else
									Console.Print( message: $"\t\t- \"{name}\"", significant: significant );

								continue;
							}

							continue;
						}

					continue;
				}

			Console.Print( );

			return;
		}
	}
}
