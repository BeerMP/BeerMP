using HutongGames.PlayMaker;
using System;

namespace BeerMP.Extensions
{
	public static class PlayMakerExtensions
	{
		public static void RemoveActionsAfter( this FsmState state, int index )
		{
			var keepCount = index + 1;
			var actions = state.Actions;

			if ( keepCount <= 0 )
			{
				state.Actions = new FsmStateAction[0];
				return;
			}

			if ( keepCount >= actions.Length ) return;

			var trimmed = new FsmStateAction[keepCount];
			Array.Copy( actions, trimmed, keepCount );
			state.Actions = trimmed;
		}
	}
}
