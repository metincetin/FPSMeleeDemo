using System;
using System.Collections;
using FPSMeleeDemo.Core;
using FPSMeleeDemo.FPS;
using UnityEngine;

namespace FPSMeleeDemo.UI
{
	public struct CursorDeflectEvent: IEvent 
	{
	}

	public struct CursorBlockEvent: IEvent 
	{
		public float RemainingBlockPower;
	}
	
    public class CursorController: MonoBehaviour
	{

        private EventBus<CursorDamageEvent>.EventHandle _cursorDamageHandle;
        private EventBus<CursorBlockEvent>.EventHandle _blockEventHandle;
        private EventBus<CursorDeflectEvent>.EventHandle _deflectEventHandle;
        private Cursor _view;

        private void OnEnable()
		{
			_cursorDamageHandle = EventBus<CursorDamageEvent>.Register(OnCursorDamage);
			_blockEventHandle = EventBus<CursorBlockEvent>.Register(OnCursorBlock);
			_deflectEventHandle = EventBus<CursorDeflectEvent>.Register(OnCursorDeflect);

			_view = GetComponent<Cursor>();
		}


        private void OnDisable()
		{
			EventBus<CursorBlockEvent>.Unregister(_blockEventHandle);
			EventBus<CursorDamageEvent>.Unregister(_cursorDamageHandle);
			EventBus<CursorDeflectEvent>.Unregister(_deflectEventHandle);
		}
		
		private void OnCursorBlock(CursorBlockEvent obj)
		{
			_view.BlockView.SetRemainingBlockRate(obj.RemainingBlockPower);
			_view.ShowBlock();
		}

        private void OnCursorDeflect(CursorDeflectEvent @event)
        {
			_view.ShowDeflect();
        }

        private void OnCursorDamage(CursorDamageEvent @event)
        {
	        _view.ShowDamage();
        }
	}
}

