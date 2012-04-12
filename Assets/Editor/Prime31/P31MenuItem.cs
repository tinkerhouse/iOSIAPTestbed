using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;


public class P31MenuItem : MonoBehaviour
{
	[MenuItem( "Prime31/Documentation site..." )]
	static void documentationSite()
	{
		openURL( "http://prime31.com/unity/docs/" );
	}
	
	
	[MenuItem( "Prime31/Plugin site..." )]
	static void pluginSite()
	{
		openURL( "http://prime31.com/unity" );
	}

	
	[MenuItem( "Prime31/Contact form..." )]
	static void contactForm()
	{
		openURL( "http://prime31.com/contactUs" );
	}
	
	
	[MenuItem( "Prime31/Info.plist additions..." )]
	static void plistAdditions()
	{
		Prime31PlistHelperWizard.CreateWizard();
	}

	
	[MenuItem( "Prime31/Prompt After Plugin Install/Enable" )]
	static void enablePrompt()
	{
		enablePromptAfterInstall( true );
	}
	
	
	[MenuItem( "Prime31/Prompt After Plugin Install/Disable" )]
	static void disablePrompt()
	{
		enablePromptAfterInstall( false );
	}
	
	
	[MenuItem( "Prime31/Update build system..." )]
	static void updateBuildSystemMenu()
	{
		var hadError = false;
		try
		{
			var baseUrl = "http://prime31.com/unity/pluginDownload.php?id=Build-System";
			var dest = System.Environment.GetFolderPath( Environment.SpecialFolder.Desktop );
			
			fetchBuildSystemFile( baseUrl, dest );
		}
		catch( Exception e )
		{
			EditorUtility.DisplayDialog( "Error Downloading Build System", e.Message, "OK" );
			hadError = true;
		}
		finally
		{
			EditorUtility.ClearProgressBar();
		}
		
		if( !hadError )
			EditorUtility.DisplayDialog( "Build System Downloaded", "Double Click the unitypackage that was placed on your desktop to update the build system", "OK" );
	}


	[MenuItem( "Prime31/Open Xcode project..." )]
	static void openXcodeProject()
	{
		// find the Xcode project file
		string path = Environment.CurrentDirectory;
		DirectoryInfo dirInfo = new DirectoryInfo( path );
		
		// dont care about these directories
		string[] invalidDirs = new string[] { "Assets", "Temp", "Library" };
		
		var possibleProjectDirs = from dir in dirInfo.GetDirectories()
									where !( (IList<string>)invalidDirs ).Contains( dir.Name )
									select dir;
		
		foreach( var dir in possibleProjectDirs )
		{
			// check the dir for "Unity-iPhone.xcodeproj"
			var possibleDir = new DirectoryInfo( dir.FullName );
			var projectFile = possibleDir.GetDirectories( "Unity-iPhone.xcodeproj" );
			
			if( projectFile.Length > 0 )
			{
				openURL( projectFile[0].FullName );
				break;
			}
		}
	}
	
	
	
	static void fetchBuildSystemFile( string url, string destination )
	{
		var www = new WWW( url );
		
		while( !www.isDone )
		{
			System.Threading.Thread.Sleep( 200 );
		}
		
		if( www.error != null )
			throw new Exception( www.error + "Try again later" );
		
		var dest = EditorUtility.SaveFilePanel( "Where would you like to save the unitypackage?", destination, "BuildSystem", "unitypackage" );
		if( dest == null || dest.Length == 0 )
			dest = destination;
		File.WriteAllBytes( dest, www.bytes );
	}
	
 
	public static void openURL( string url )
	{
	    try
	    {
            ProcessStartInfo pInfo = new ProcessStartInfo( "open", url );
            Process.Start( pInfo );
	    }
	    catch( Exception e )
	    {
			UnityEngine.Debug.Log( String.Format( "Error occurred when trying to open URL {0}. Error: {1}", url, e.Message ) );
	    }
	}
	
	
	public static void enablePromptAfterInstall( bool enable )
	{
		// find all the config.plist files in plugin directories
		string basePath = Path.Combine( Application.dataPath, "Editor" );
		var dirInfo = new DirectoryInfo( basePath );
		
		var pluginDirs = from dir in dirInfo.GetDirectories()
							let files = dir.GetFiles( "config.plist" )
							where files.Length == 1
							select files[0];
		
		// loop through our pluginDirs
		foreach( var dir in pluginDirs )
		{
			if( !File.Exists( dir.FullName ) )
				continue;
				
			// initialize the hashtable and plistKeys
			Hashtable plistContents = new Hashtable();
			
			PListEditor.loadPlistFromFile( dir.FullName, plistContents );
			
			if( plistContents.ContainsKey( "neverShowCompletedMessage" ) )
			{
				plistContents["neverShowCompletedMessage"] = !enable;
				PListEditor.savePlistToFile( dir.FullName, plistContents );
			}
		}
	}

}
