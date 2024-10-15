using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace FPSMeleeDemo.UI.Views
{
	public class Card : VisualElement
	{
		public new class UxmlFactory : UxmlFactory<Cursor, UxmlTraits> { }

		public Card()
		{
		}
	}
}
