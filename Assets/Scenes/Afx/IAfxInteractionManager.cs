using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scenes.Afx
{
    public interface IAfxInteractionManager
    {
        void PlayFx(string afxEffectId);
        void EmitEventToAfxAsync(string afxEventId);
    }
}
