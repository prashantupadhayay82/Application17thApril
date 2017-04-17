using System;
using System.Collections;
using System.Security;
using System.Security.Principal;
using System.Runtime.Serialization;

namespace CustomAuth
{
    /// <summary>
    /// Represents a Custom Principal
    /// </summary>
    [Serializable]
    public class CustomPrincipal : IPrincipal, ISerializable
    {
        private IIdentity identity;
        private ArrayList roles;

        /// <summary>
        /// Gets the CustomIdentity of the user represented by the current CustomPrincipal.
        /// </summary>
        public IIdentity Identity
        {
            get { return identity; }
            set { identity = value; }
        }

        /// <summary>
        /// Initializes a new instance of the GenericPrincipal class from a CustomIdentity and an ArrayList of role names 
        /// to which the user represented by that CustomIdentity belongs
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rolesArray"></param>
        public CustomPrincipal(IIdentity id, ArrayList rolesArray)
        {
            identity = id;
            roles = rolesArray;
        }

        /// <summary>
        /// Determines whether the current CustomPrincipal belongs to the specified role.
        /// </summary>
        /// <param name="role">The name of the role for which to check membership</param>
        /// <returns>true if the current CustomPrincipal is a member of the specified role; otherwise, false.</returns>
        public bool IsInRole(string role)
        {
            return roles.Contains(role);
        }

        #region ISerializable Members
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            //State will be CrossAppDomain for serialization induced by WebDev.WebServer
            if (context.State.Equals(StreamingContextStates.CrossAppDomain))
            {
                var identity = (IIdentity)this.Identity;

                var generic = new GenericPrincipal(identity, null); ;

                info.SetType(generic.GetType());

                var serializableMembers = FormatterServices.GetSerializableMembers(generic.GetType());
                var serializableValues = FormatterServices.GetObjectData(generic, serializableMembers);

                for (int i = 0; i < serializableMembers.Length; i++)
                    info.AddValue(serializableMembers[i].Name, serializableValues[i]);
            }
            else
            {
                throw new InvalidOperationException("Serialization not supported");
            }
        }
        #endregion
    }
}
