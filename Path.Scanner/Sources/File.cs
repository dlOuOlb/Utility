using S = System;
using G = System.Collections.Generic;
using static System.Linq.ParallelEnumerable;

namespace Utility
{
	public static class File: S.Object
	{
		public static class Name: S.Object
		{
			private static readonly S.Boolean isCaseSensitive = @"." != S.IO.Path.GetRelativePath( relativeTo: "A", path: "a" );
			private static readonly S.StringComparer comparer = isCaseSensitive ? S.StringComparer.InvariantCulture : S.StringComparer.InvariantCultureIgnoreCase;

			private static Value Get<Value>( Value value ) => value;
			private static S.String GetName( S.IO.FileSystemInfo information ) => information.Name;

			private static G.SortedSet<S.String>? Scan( S.String directory ) => S.IO.Directory.Exists( path: directory ) ? new( comparer: comparer, collection: new S.IO.DirectoryInfo( path: directory ).EnumerateFiles( ).AsParallel( ).Select( selector: GetName ) ) : null;
			public static G.SortedDictionary<S.String, G.SortedSet<S.String>?> Scan( G.IEnumerable<S.String> directories ) => new( comparer: comparer, dictionary: directories.AsParallel( ).ToDictionary( comparer: comparer, keySelector: Get, elementSelector: Scan ) );

			public static S.Linq.ParallelQuery<S.String> Intersect( G.IEnumerable<S.String> former, G.IEnumerable<S.String> latter ) => former.AsParallel( ).Intersect( second: latter.AsParallel( ), comparer: comparer );
		}

		public static class Data: S.Object
		{
			private static S.Boolean Equals<Value>( G.IEnumerable<Value> former, G.IEnumerable<Value> latter ) => former.AsParallel( ).SequenceEqual( second: latter.AsParallel( ) );
			private static S.Boolean Equals( S.String former, S.String latter ) => Equals( former: S.IO.File.ReadAllBytes( path: former ), latter: S.IO.File.ReadAllBytes( path: latter ) );
			public static S.Boolean Equals( S.IO.FileInfo former, S.IO.FileInfo latter )
			{
				using( former.OpenRead( ) )
				using( latter.OpenRead( ) )
					return former.Length == former.Length && Equals( former: former.FullName, latter: latter.FullName );
			}
		}
	}
}
