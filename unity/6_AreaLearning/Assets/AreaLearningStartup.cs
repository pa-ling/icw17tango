using System.Collections;
using UnityEngine;
using Tango;

public class AreaLearningStartup : MonoBehaviour, ITangoLifecycle
{
	private TangoApplication m_tangoApplication;

	public void Start()
	{
		m_tangoApplication = FindObjectOfType<TangoApplication>(); //initialize
		if (m_tangoApplication != null)
		{
			m_tangoApplication.Register(this); //register to receive callbacks
			m_tangoApplication.RequestPermissions();
		}
	}

	public void OnTangoPermissions(bool permissionsGranted)
	{
		if (permissionsGranted)
		{//get most recent area description
			AreaDescription[] list = AreaDescription.GetList();
			AreaDescription mostRecent = null;
			AreaDescription.Metadata mostRecentMetadata = null;
			if (list.Length > 0)
			{
				// Find and load the most recent Area Description
				//TODO: let user choose which area description should be taken
				mostRecent = list[0];
				mostRecentMetadata = mostRecent.GetMetadata();
				foreach (AreaDescription areaDescription in list)
				{
					AreaDescription.Metadata metadata = areaDescription.GetMetadata();
					if (metadata.m_dateTime > mostRecentMetadata.m_dateTime)
					{
						mostRecent = areaDescription;
						mostRecentMetadata = metadata;
					}
				}

				m_tangoApplication.Startup(mostRecent); //connect area description with tango
			}
			else
			{
				// No Area Descriptions available.
				Debug.Log("No area descriptions available.");
				//TODO:Correct error handling or insert learning mode
			}
		}
	}

	public void OnTangoServiceConnected()
	{
	}

	public void OnTangoServiceDisconnected()
	{
	}
}
