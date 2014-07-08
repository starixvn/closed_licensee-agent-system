namespace LicenseeAgent.Tests.Fakes
{
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Web;
    using System.Web.SessionState;

    /// <summary>
    /// FakeHttpSessionState class.
    /// </summary>
    public class FakeHttpSessionState : HttpSessionStateBase
    {
        private readonly SessionStateItemCollection _sessionItems;

        public FakeHttpSessionState(SessionStateItemCollection sessionItems)
        {
            _sessionItems = sessionItems ?? new SessionStateItemCollection();
        }

        public override int Count
        {
            get
            {
                return _sessionItems.Count;
            }
        }

        public override NameObjectCollectionBase.KeysCollection Keys
        {
            get
            {
                return _sessionItems.Keys;
            }
        }

        public override string SessionID
        {
            get
            {
                return "sessionid";
            }
        }

        public override object this[string name]
        {
            get
            {
                return _sessionItems[name];
            }

            set
            {
                _sessionItems[name] = value;
            }
        }

        public override object this[int index]
        {
            get
            {
                return _sessionItems[index];
            }

            set
            {
                _sessionItems[index] = value;
            }
        }

        public override void CopyTo(Array array, int index)
        {
            ((ICollection)_sessionItems).CopyTo(array, index);
        }

        public override IEnumerator GetEnumerator()
        {
            return _sessionItems.GetEnumerator();
        }

        public override void Add(string name, object value)
        {
            _sessionItems[name] = value;
        }

        public override void Remove(string name)
        {
            _sessionItems.Remove(name);
        }
    }
}