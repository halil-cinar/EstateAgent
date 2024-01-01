using EstateAgent.Dto.Enums;
using EstateAgent.Dto.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateAgent.Dto.Result
{
    public class BussinessLayerResult<T>

    {
        public T Result { get; set; }
        public List<ErrorMessage> ErrorMessages { get; set; }

        public ResultStatus ResultStatus
        {
            get
            {
                if (ErrorMessages.Count > 0)
                {
                    return ResultStatus.Error;
                }
                return ResultStatus.Success;
            }
        }

        public BussinessLayerResult()
        {
            ErrorMessages= new List<ErrorMessage>();
        }

        public void AddError(ErrorMessageCode code,string message)
        {
            ErrorMessages.Add(new ErrorMessage { ErrorCode=code,Message=message});
        }

    }
}
