using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace API
{
    public class ManualValidationFilterAttribute : Attribute, IActionModelConvention
    {
        public void Apply(ActionModel action)
        {
            for (var i = 0; i < action.Filters.Count; i++)
            {
                // check is action.Filters[i] is of type ModelStateInvalidFilter or a derived type
                if (action.Filters[i] is ModelStateInvalidFilter || 
                    action.Filters[i].GetType().Name == "ModelStateInvalidFilterFactory")
                {
                    action.Filters.RemoveAt(i);
                    break;
                }
            }
        }
    }
}
