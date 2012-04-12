using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;



class Prime31PlistHelperWizard : ScriptableWizard
{
    public string MinimumOSVersion = null;
    public bool UIFileSharingEnabled = false;
    public string[] CFBundleURLSchemes = null;
    
    private Hashtable plistContents;
    private string plistFileName = "plistAdditions.plist";
    private string filePath;
    
    
    public static void CreateWizard()
   	{
   		// get the contents of the plist or create a new one
		var helper = ScriptableWizard.DisplayWizard<Prime31PlistHelperWizard>( "Info.plist Additions", "Save Changes", "Cancel" );
		helper.getPlistContents();
		helper.minSize = new Vector2( 500, 200 );
		helper.maxSize = new Vector2( 500, 1000 );
    }
   

	// Called when the 'save changes' button is pressed
    void OnWizardCreate()
    {
        // fill out the hashtable with the new values
        var ht = new Hashtable();
        
        // add an entry with all the key names
        var plistKeys = getAllPublicIvarNames();
        ht["plistKeys"] = plistKeys;
        
        if( MinimumOSVersion != null && MinimumOSVersion != string.Empty && MinimumOSVersion.Length > 0 )
		{
			ht["MinimumOSVersion"] = MinimumOSVersion;
		}
		else
		{
			ht.Remove( "MinimumOSVersion" );
		}
        
        ht["UIFileSharingEnabled"] = this.UIFileSharingEnabled;

        if( CFBundleURLSchemes != null && CFBundleURLSchemes.Length > 0 )
        {
        	var scheme = new Hashtable();
        	scheme["CFBundleURLName"] = string.Empty;
        	scheme["CFBundleURLSchemes"] = new ArrayList( CFBundleURLSchemes );
        	
        	var urlTypes = new ArrayList();
        	urlTypes.Add( scheme );
        	ht["CFBundleURLTypes"] = urlTypes;
        }
		
		foreach( var k in ht.Keys )
		{
			Debug.Log( k );
		}
        
        PListEditor.savePlistToFile( filePath, ht );
    }
    
    
    void OnWizardOtherButton()
    {
    	this.Close();
    }


	// Context sensitive help
    void OnWizardUpdate()
    {
        helpString = "Set the plist options that will be merged into your Info.plist file";
    }
	
	
	// grabs the contents of the plist and sets them in the ivar
	public void getPlistContents()
	{
		// initialize the hashtable and plistKeys
		plistContents = new Hashtable();
		
		// get the contents of the plist file if it exists
		filePath = Path.Combine( Application.dataPath, "Editor/Prime31/" + plistFileName );
		
		if( File.Exists( filePath ) )
		{
			PListEditor.loadPlistFromFile( filePath, plistContents );
			
			// set any keys that we have present
			var t = typeof( Prime31PlistHelperWizard );
			foreach( var info in t.GetFields() )
			{
				if( plistContents.ContainsKey( info.Name ) )
				{
					info.SetValue( this, plistContents[info.Name] );
				}
				else if( plistContents.ContainsKey( "CFBundleURLTypes" ) ) // special case for url schemes
				{
					var values = (ArrayList)plistContents["CFBundleURLTypes"];
					var ht = values[0] as Hashtable;
					var listOfSchemes = ht["CFBundleURLSchemes"] as ArrayList;
					
					var extractedSchemes = new List<string>();
					foreach( string scheme in listOfSchemes )
						extractedSchemes.Add( scheme );
					
					CFBundleURLSchemes = extractedSchemes.ToArray();
				}
			}
		}
	}
	
	
	// initializes and saves off all the available plistKeys (public ivars)
	public ArrayList getAllPublicIvarNames()
	{
		var arr = new ArrayList();
		
		var t = typeof( Prime31PlistHelperWizard );
		foreach( var info in t.GetFields() )
		{
			// swap CFBundleURLTypes for CFBundleURLSchemes
			if( info.Name == "CFBundleURLSchemes" )
				arr.Add( "CFBundleURLTypes" );
			else
				arr.Add( info.Name );
		}
		
		return arr;
	}

}
