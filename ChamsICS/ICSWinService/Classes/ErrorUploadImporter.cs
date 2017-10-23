using ChamsICSLib.Data;
using ChamsICSLib.Utilities;
using ICSWinService.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICSWinService.Classes
{
    class ErrorUploadImporter: ServicesAdapter
    {
        public ErrorUploadImporter(){
            initializeSeviceSettings();
        }

        internal override int RunServiceOpperation(string dataBatch)
        {
            int result = 0;//Number of Funds Sources Processed;

            int batchSize = 0;
            Int32.TryParse(dataBatch, out batchSize);
            IList<String> LogBatch = LoadLogBatch(batchSize);

            UpdateProcessLog(string.Format("Loaded {0} batches", LogBatch.Count()));

            foreach (var item in LogBatch)
            {
                UpdateProgressCompleted(LogBatch.Count());
                UpdateProcessLog(string.Format("Processing {0}...", item));

                //Load Error Uploads in Batch Folder
                List<string> batchfiles = LoadBatchFiles(item);
                UpdateProcessLog(string.Format("Loaded {0} file(s) from {1}...", batchfiles.Count(), item));
                foreach (string file in batchfiles)
                {
                    try
                    {
                        UpdateProcessLog(string.Format("Processing {0}...", file));

                        //Read All lines in the ErrorLog file
                        string[] fileLines = File.ReadAllLines(file);

                        //Read First Line Which Contails Error Message During Upload
                        string firstLine = fileLines[0];

                        //Read Other Lines in Error Log File Except First Line
                        string[] errorUploadData = fileLines.Skip(1).ToArray();

                        //Build a String with errorMessage Line
                        string joinedErrorUploadData = string.Join(Environment.NewLine, errorUploadData);
                        //Trim dateStamp after String
                        string[] magicSpliter = { "</UploadTransactionReq>" };
                        joinedErrorUploadData = (joinedErrorUploadData.Split(magicSpliter, StringSplitOptions.None)[0]) + magicSpliter[0];

                        //Deserialise ErrorUpload Data to Object
                        UploadTransactionReq ErrorUploadData = (UploadTransactionReq)XMLHelper.deserializeXMLStringToObject(joinedErrorUploadData, new UploadTransactionReq().GetType());

                        //Save Error Upload In Database
                        if (UploadExceptionToDbv2(firstLine, ErrorUploadData))
                        {
                            //Move File to Another Folder
                            string destinationFile = new FileInfo(failed_upload_path + "_Processed\\" + new FileInfo(file).Name).FullName;
                            string processedFile = new FileInfo(file).FullName;
                            File.Move(processedFile, destinationFile);
                        }
                    }
                    catch (Exception exp)
                    {
                        UpdateProcessLog(exp.ToString());
                    }                  
                }
            }
            return result;
        }

        private List<string> LoadBatchFiles(string item)
        {
            return Directory.GetFiles(item,"*.xml").ToList();
        }

        private IList<string> LoadLogBatch(int batchSize)
        {
            return Directory.GetDirectories(failed_upload_path);
        }

        internal bool UploadExceptionToDb(string msg, UploadTransactionReq req)
        {
            try
            {
                using (CICSEntities _db = new CICSEntities())
                {
                    string TerminalCode = req.TransactionCode.Substring(4, 6);
                    string AgentCode = req.TransactionCode.Substring(0, 4);
                    Agent agent = FindAgentByCode(AgentCode);
                    Terminal terminal = FindTerminalByCode(TerminalCode);
                    Client client = null;
                    if (agent != null)
                        client = _db.Clients.FirstOrDefault(x => x.Id == agent.ClientId);

                    TransactionLogException tLog = new TransactionLogException();

                    tLog.ClientId = client != null ? client.Id.ToString() : null;
                    tLog.AgentId = agent != null ? agent.Id.ToString() : null;
                    tLog.TerminalId = terminal != null ? terminal.Id.ToString() : null;
                    tLog.ResidentId = req.ResidentId;
                    tLog.Address = req.Address;
                    tLog.Amount = req.Amount;
                    tLog.Code = req.TransactionCode;
                    tLog.DateOfBirth = req.DateOfBirth;
                    tLog.Email = req.Email;
                    tLog.FirstName = req.FirstName;
                    tLog.Gender = req.Gender;
                    tLog.Lastname = req.LastName;
                    tLog.MiddleName = req.MiddleName;
                    tLog.PhoneNumber = req.PhoneNumber;
                    tLog.RevenueCode = req.RevenueCode;
                    tLog.TransactionDate = req.TransactionDate.ToString();
                    tLog.UploadDate = DateTime.Now.ToString();
                    tLog.PaymentReference = req.PaymentReference;
                    tLog.Status = "0";
                    tLog.UploadError = msg;

                    _db.TransactionLogExceptions.Add(tLog);
                    try
                    {
                        _db.SaveChanges();
                    }
                    catch (System.Data.Entity.Core.UpdateException e)
                    {

                    }

                    catch (System.Data.Entity.Infrastructure.DbUpdateException ex) //DbContext
                    {
                        Console.WriteLine(ex.InnerException);
                    }

                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.InnerException);
                        throw;
                    }
                }
                return true;
            }
            catch (Exception exp)
            {
                return false;
            }
        }

        internal bool UploadExceptionToDbv2(string msg, UploadTransactionReq req)
        {
            try
            {
                using (CICSEntities _db = new CICSEntities())
                {

                    string TerminalCode = req.TransactionCode.Substring(4, 6);
                    string AgentCode = req.TransactionCode.Substring(0, 4);

                    Agent agent = FindAgentByCode(AgentCode);
                    Terminal terminal = FindTerminalByCode(TerminalCode);

                    ChamsICSLib.Data.Client client = null;
                    if (agent != null)
                        client = _db.Clients.FirstOrDefault(x => x.Id == agent.ClientId);

                    string result = string.Empty;
                    TransactionUploadError tLog = new TransactionUploadError();
                    DateTime dateParser = DateTime.Now;

                    if (client != null)
                        tLog.ClientId = client.Id;
                    if (agent != null)
                        tLog.AgentId = agent.Id;
                    if (terminal != null)
                        tLog.TerminalId = terminal.Id;

                    tLog.ResidentId = req.ResidentId;
                    tLog.Address = req.Address;

                    tLog.Amount = Convert.ToDecimal(req.Amount);
                    tLog.Code = req.TransactionCode;

                    tLog.DateOfBirth = DateTime.TryParse(req.DateOfBirth, out dateParser) ? dateParser : DateTime.Now;

                    tLog.Email = req.Email;
                    tLog.FirstName = req.FirstName;
                    tLog.Gender = req.Gender;
                    tLog.Lastname = req.LastName;
                    tLog.MiddleName = req.MiddleName;
                    tLog.PhoneNumber = req.PhoneNumber;
                    tLog.RevenueCode = req.RevenueCode;

                    tLog.TransactionDate = DateTime.TryParse(req.TransactionDate, out dateParser) ? dateParser : DateTime.Now;

                    tLog.UploadDate = DateTime.Now;
                    tLog.PaymentReference = req.PaymentReference;
                    tLog.Status = 0;
                    tLog.UploadError = msg;

                    _db.TransactionUploadErrors.Add(tLog);

                    try
                    {
                        _db.SaveChanges();
                    }
                    catch (System.Data.Entity.Core.UpdateException e)
                    {

                    }

                    catch (System.Data.Entity.Infrastructure.DbUpdateException ex) //DbContext
                    {
                        Console.WriteLine(ex.InnerException);
                    }

                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.InnerException);
                        throw;
                    }
                    return true;
                }
            }
            catch (Exception exp)
            {
                return false;
            } 
        }

        internal Terminal FindTerminalByCode(string terminalCode)
        {
            return db.Terminals.FirstOrDefault(x => x.Code == terminalCode);
        }

        internal Agent FindAgentByCode(string agentCode)
        {
            return db.Agents.FirstOrDefault(x => x.Code == agentCode.Trim());
        }
    }
}
