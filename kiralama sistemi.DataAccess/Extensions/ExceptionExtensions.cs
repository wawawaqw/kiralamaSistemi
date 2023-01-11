using kiralamaSistemi.DataAccess.Sevices;
using kiralamaSistemi.Entities.Enums;
using kiralamaSistemi.Entities.Extensions;
using Microsoft.Data.SqlClient;
using System.Diagnostics;

namespace kiralamaSistemi.DataAccess.Extensions
{
    public static class ExceptionExtensions
    {
        public static void WriteToXml(this Exception ex, Error? error = null)
        {
            var logModel = new List<XmlLogModel>();
            XmlLogger xml = new();
            if (error != null)
            {
                logModel.Add(new XmlLogModel()
                {
                    Message = error.Description,
                    EventId = error.Code,
                    Formatter = error.Key?.GetEnumTitle(),
                    LineNumber = "",
                    Source = error.Title,
                    StackTrace = ""
                });
            }
            logModel.AddRange(ex.GetXmlModelFromException());
            xml.WriteXDocument(logModel);
        }
        public static List<XmlLogModel> GetXmlModelFromException(this Exception ex)
        {
            var logModelList = new List<XmlLogModel>();
            if (ex is SqlException exception)
            {
                var st = new StackTrace(ex, true);
                var frame = st.GetFrame(0);
                int line = frame?.GetFileLineNumber() ?? 0;

                for (int i = 0; i < exception.Errors.Count; i++)
                {
                    logModelList.Add(new XmlLogModel()
                    {
                        Message = exception.Errors[i].Message,
                        EventId = exception.Errors[i].LineNumber.ToString(),
                        Formatter = exception.Errors[i].Message,
                        LineNumber = line.ToString(),
                        Source = exception.Errors[i].Source,
                        StackTrace = st.ToString()
                    });
                }
            }
            else if (ex is OzelException ozelException)
            {
                var st = new StackTrace(ex, true);
                var frame = st.GetFrame(0);
                int line = frame?.GetFileLineNumber() ?? 0;

                if (ozelException.Errors == null)
                {
                    logModelList.Add(new XmlLogModel()
                    {
                        Message = ozelException.Message,
                        EventId = line.ToString(),
                        Formatter = ozelException.Data.Keys.ToString(),
                        LineNumber = line.ToString(),
                        Source = ozelException.Source,
                        StackTrace = st.ToString()
                    });
                }
                else
                {
                    logModelList.Add(new XmlLogModel()
                    {
                        Message = ozelException.Message + "|" + string.Join("|", ozelException.Errors.Select(i => i.Code + " " + i.Title)),
                        EventId = line.ToString(),
                        Formatter = string.Join("|", ozelException.Errors.Select(i => i.Description)),
                        LineNumber = line.ToString(),
                        Source = ozelException.Source,
                        StackTrace = st.ToString()
                    });
                }
            }
            else
            {
                var st = new StackTrace(ex, true);
                var frame = st.GetFrame(0);
                int line = frame?.GetFileLineNumber() ?? 0;

                logModelList.Add(new XmlLogModel()
                {
                    Message = ex.Message,
                    EventId = line.ToString(),
                    Formatter = ex.Message,
                    LineNumber = line.ToString(),
                    Source = ex.Source,
                    StackTrace = st.ToString()
                });
            }

            if (ex.InnerException != null)
            {
                logModelList.AddRange(ex.InnerException.GetXmlModelFromException());
            }

            return logModelList;
        }
        public static List<Error> GetListError(this Exception ex)
        {
            var ErrorList = new List<Error>();
            if (ex is SqlException exception)
            {
                for (int i = 0; i < exception.Errors.Count; i++)
                {
                    ErrorList.Add(new Error(null, exception.Errors[i].Message) { Key = EnumErrorTypes.danger, Title = ex.Message });
                }
            }
            else if (ex is OzelException ozelOxception)
            {
                if (ozelOxception.Errors == null)
                {
                    ErrorList.Add(new Error(null, ozelOxception.Message) { Key = EnumErrorTypes.danger, Title = ex.Message });
                }
                else
                {
                    ErrorList.AddRange(ozelOxception.Errors);
                }
            }
            else
            {
                ErrorList.Add(new Error(null, ex.Message) { Key = EnumErrorTypes.danger, Title = ex.Message });
            }

            return ErrorList;
        }
    }
}
