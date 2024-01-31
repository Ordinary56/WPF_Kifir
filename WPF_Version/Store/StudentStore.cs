using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Kifir.Model;

namespace WPF_Kifir.Store
{
    public interface IMediator
    {
        event EventHandler<object> ObjectSent;
        void SendMessage(object sender, object obj);
    }
    public class Mediator : IMediator
    {
        public event EventHandler<object>? ObjectSent;

        public void SendMessage(object sender, object obj)
        {
                OnObjectSent(sender, obj);
        }
        protected virtual void OnObjectSent(object sender, object obj)
        {
            ObjectSent?.Invoke(sender,obj);
        }
    }
}
