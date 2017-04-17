using System;
using System.Security.Principal;
using System.Web;
using System.Runtime.Serialization;

namespace CustomAuth
{
    /// <summary>
    /// Represents the Identity of a User. 
    /// Stores the details of a User. 
    /// Implements the IIDentity interface.
    /// </summary>
    [Serializable]
    public class CustomIdentity : IIdentity, ISerializable
    {
        # region private variables
        private UserContextModel _User;
        private string _Name = string.Empty;
        #endregion

        # region constructors
        /// <summary>
        /// The default constructor initializes any fields to their default values.
        /// </summary>
        public CustomIdentity()
        {
            this._User = null;
            this._Name = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the CustomIdentity class with the passed parameters
        /// </summary>
        public CustomIdentity(dynamic model)
        {
            this._User = model;
            this._Name = Convert.ToString(model.UserCode);
        }

        public static CustomIdentity EmptyIdentity
        {
            get { return new CustomIdentity(); }
        }
        #endregion

        # region properties
        /// <summary>
        /// Gets the Authentication Type
        /// </summary>
        public string AuthenticationType { get { return "Custom"; } }

        /// <summary>
        /// Indicates whether the User is authenticated
        /// </summary>
        public bool IsAuthenticated { get { return true; } }

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        public UserContextModel User
        {
            get { return _User; }
            set { _User = value; }
        }
        #endregion

        #region ISerializable Members
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            //State will be CrossAppDomain for serialization induced by WebDev.WebServer
            if (context.State.Equals(StreamingContextStates.CrossAppDomain))
            {
                var generic = new GenericIdentity(this.Name, this.AuthenticationType);
                info.SetType(generic.GetType());
                var serializableMembers = FormatterServices.GetSerializableMembers(generic.GetType());
                var serializableValues = FormatterServices.GetObjectData(generic, serializableMembers);
                for (int i = 0; i < serializableMembers.Length; i++)
                    info.AddValue(serializableMembers[i].Name, serializableValues[i]);
            }
            else
                throw new InvalidOperationException("Serialization not supported");
        }
        #endregion
    }

    public class UserContext
    {
        public static CustomIdentity CurrentUser
        {
            get
            {
                if (HttpContext.Current.User.Identity is CustomIdentity)
                    return (CustomIdentity)HttpContext.Current.User.Identity;
                return CustomIdentity.EmptyIdentity;
            }
        }
    }
}