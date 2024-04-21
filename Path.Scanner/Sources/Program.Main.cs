using S = System;

namespace Utility
{
	public static partial class Program: S.Object
	{
		private const S.String path = nameof( path );

		public static void Main( S.String[ ] arguments )
		{
			try
			{
				if( S.Environment.GetEnvironmentVariable( variable: path ) is not S.String variable )
					throw new S.NotSupportedException( message: $"The environment variable key \"{path}\" is not found." );
				else if( 0 == arguments.Length )
					Console.Print( significant: true, message: @"Usage: Drag and drop a folder on this executable." );
				else
					Verify( arguments: arguments, variables: variable.Split( separator: ';', options: S.StringSplitOptions.RemoveEmptyEntries ) );
			}
			catch( S.Exception exception )
			{
				Console.Print( error: true, significant: true, message: exception.ToString( ) );
				Console.Print( error: true );
			}
			finally
			{
				Console.Print( message: @"Press any key to continue..." );
				_ = S.Console.ReadKey( intercept: true );
			}

			return;
		}
	}
}
