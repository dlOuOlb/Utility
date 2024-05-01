using S = System;
using G = System.Collections.Generic;
using static System.Linq.Enumerable;
using P = System.Linq.ParallelEnumerable;

namespace Utility
{
	public static class File: S.Object
	{
		public static class Name: S.Object
		{
			private static readonly S.Boolean isCaseSensitive = @"." != S.IO.Path.GetRelativePath( relativeTo: "A", path: "a" );
			private static readonly S.StringComparer comparer = isCaseSensitive ? S.StringComparer.InvariantCulture : S.StringComparer.InvariantCultureIgnoreCase;

			private static S.String GetName( S.IO.FileSystemInfo information ) => information.Name;

			private static G.KeyValuePair<S.String, G.SortedSet<S.String>?> Scan( S.String directory ) => new( key: directory, value: S.IO.Directory.Exists( path: directory ) ? new( comparer: comparer, collection: new S.IO.DirectoryInfo( path: directory ).EnumerateFiles( ).Select( selector: GetName ) ) : null );
			public static G.IEnumerable<G.KeyValuePair<S.String, G.SortedSet<S.String>?>> Scan( G.IEnumerable<S.String> directories ) => directories.Select( selector: Scan );

			public static G.IEnumerable<S.String> Intersect( G.IEnumerable<S.String> former, G.IEnumerable<S.String> latter ) => former.Intersect( second: latter, comparer: comparer );
		}

		public static class Data: S.Object
		{
			private static S.Boolean Equals<Value>( G.IEnumerable<Value> former, G.IEnumerable<Value> latter ) => P.SequenceEqual( first: P.AsParallel( source: former ), second: P.AsParallel( source: latter ) );
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
