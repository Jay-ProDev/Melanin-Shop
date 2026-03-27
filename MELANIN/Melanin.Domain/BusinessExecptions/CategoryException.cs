using System;
using System.Collections.Generic;
using System.Text;

namespace Melanin.Domain.BusinessExecptions
{
    public class CategoryException : Exception
    {
        public CategoryException(string? message) : base(message) { }
    }

    public class CategoryNotFoundException : CategoryException
    {
        public CategoryNotFoundException()
            : base("Catégorie introuvable") { }

        public CategoryNotFoundException(int id)
            : base($"Catégorie avec l'id {id} introuvable") { }
    }

    public class CategoryAlreadyExistsException : CategoryException
    {
        public CategoryAlreadyExistsException(string slug)
            : base($"Une catégorie avec le slug '{slug}' existe déjà.") { }
    }
}