using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.XRM.DEV.CustomAction
{
    public class GetAccountNumberByName : CodeActivity
    {
        //[RequiredArgument]
        //[Input("String input")]
        //public InArgument<string> StringInput { get; set; }

        [Output("Int output")]
        public OutArgument<int> IntOutput { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            //string input = StringInput.Get(context);
            IntOutput.Set(context, 10);
        }
    }
}