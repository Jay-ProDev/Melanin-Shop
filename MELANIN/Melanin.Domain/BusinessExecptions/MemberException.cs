using System;
using System.Collections.Generic;
using System.Text;

namespace Melanin.Domain.BusinessExecptions
{
    public class MemberException : Exception
    {
        public MemberException(string? message) : base(message) { }
    }

    public class MemberNotFoundException : MemberException
    {
        public MemberNotFoundException()
            : base("Membre introuvable") { }

        public MemberNotFoundException(int id)
            : base($"Membre avec l'id {id} introuvable") { }
    }

    public class MemberAlreadyExistsException : MemberException
    {
        public MemberAlreadyExistsException()
            : base("Cet email est déjà utilisé") { }
    }

    public class MemberBadCredentialException : MemberException
    {
        public MemberBadCredentialException()
            : base("Email ou mot de passe incorrect") { }
    }
}
