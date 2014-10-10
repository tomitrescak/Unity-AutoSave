using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Threading;
using System;
public class Threader 
{
	static ICollection<Action> m_deferredActions = new List<Action>();
	static ICollection<Action> m_currentActions = new List<Action>();
	static object m_lock;

	public static void Enable()
	{
		m_lock = new object();

		EditorApplication.update += () =>
		{
			lock (m_lock)
			{
				var tmp = m_deferredActions;
				m_deferredActions = m_currentActions;
				m_currentActions  = tmp;
			}
			foreach (var action in m_currentActions)
			{
				try
				{
					action();
				}
				catch { }
			}
			m_currentActions.Clear();
		};
	}
	public static void RunAsync(Action action)
	{
		ThreadPool.QueueUserWorkItem((_) =>
	    {
			try
			{
				action();
			}
			catch { }
		});
	}
	public static void RunOnMain(Action action)
	{
		lock (m_lock)
		{
			m_deferredActions.Add(action);
		}
	}
}