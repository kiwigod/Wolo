using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Net.Security;
using System.Net.Http;
using System.Threading.Tasks;
using System;

namespace HoneymoonShop.Controllers
{
    public static class Message
    {
        
        public static async Task<string> Send(
                string name,
                string email,
                string phone,
                string department,
                string message,
                string server,
                int port,
                string smtpServerAccountName,
                string smtpServerUsername,
                string smtpServerPassword,
                string destinationName,
                string destinationEmail,
                string destinationSubjectLine)
        {
            if (port == 465)
            {
                return await SendMessageOnPort465(
                    name,
                    email,
                    phone,
                    department,
                    message,
                    server,
                    port,
                    smtpServerAccountName,
                    smtpServerUsername,
                    smtpServerPassword,
                    destinationName,
                    destinationEmail,
                    destinationSubjectLine);
            }
            else
            {
                return await SendMessageOnPort587(
                    name,
                    email,
                    phone,
                    department,
                    message,
                    server,
                    port,
                    smtpServerAccountName,
                    smtpServerUsername,
                    smtpServerPassword,
                    destinationName,
                    destinationEmail,
                    destinationSubjectLine);
            }
        }
        
        private static string ProcessServerResponse(string step, string serverReply, string successCode)
        {
            // x000 return = the server response was empty
            // x001 return = the server responded with fewer than 3 characters
            // xyyy return = the server responded with a code that doesn't match the successCode supplied
            if (string.IsNullOrEmpty(serverReply))
            {
                return step + "000";
            }
            else
            {
                if (serverReply.StartsWith(successCode))
                {
                    return string.Empty;
                }
                else
                {
                    if (serverReply.Length >= 3)
                    {
                        return step + serverReply.Substring(0, 3);
                    }
                    else
                    {
                        return step + "001";
                    }
                }
            }
        }
        
        private static void CheckServerReply(string serverReply)
        {
            if (serverReply.Length != 0)
            {
                throw new HttpRequestException();
            }
        }

        private static async Task<string> SendMessageOnPort465(
                string name,
                string email,
                string phone,
                string department,
                string message,
                string server,
                int port,
                string smtpServerAccountName,
                string smtpServerUsername,
                string smtpServerPassword,
                string destinationName,
                string destinationEmail,
                string destinationSubjectLine)
        {
            string serverReply = string.Empty;
            try
            {
                using (var client = new TcpClient())
                {
                    await client.ConnectAsync(server, port);
                    using (var stream = new SslStream(client.GetStream(), false))
                    {
                        await stream.AuthenticateAsClientAsync(server);
                        using (var reader = new StreamReader(stream))
                        using (var writer = new StreamWriter(stream) { AutoFlush = true })
                        {
                            serverReply = ProcessServerResponse("1", reader.ReadLine(), "220");
                            CheckServerReply(serverReply);
                            
                            writer.WriteLine("HELO " + server);
                            serverReply = ProcessServerResponse("2", reader.ReadLine(), "250");
                            CheckServerReply(serverReply);
                            
                            writer.WriteLine("AUTH LOGIN");
                            serverReply = ProcessServerResponse("3", reader.ReadLine(), "334");
                            CheckServerReply(serverReply);
                            
                            string username = smtpServerUsername;
                            var plainTextBytes1 = Encoding.UTF8.GetBytes(username);
                            string base64Username = Convert.ToBase64String(plainTextBytes1);
                            writer.WriteLine(base64Username);
                            serverReply = ProcessServerResponse("4", reader.ReadLine(), "334");
                            CheckServerReply(serverReply);
                            
                            string password = smtpServerPassword;
                            var plainTextBytes2 = Encoding.UTF8.GetBytes(password);
                            string base64Password = Convert.ToBase64String(plainTextBytes2);
                            writer.WriteLine(base64Password);
                            serverReply = ProcessServerResponse("5", reader.ReadLine(), "235");
                            CheckServerReply(serverReply);
                            
                            writer.WriteLine("MAIL FROM:<" + smtpServerUsername + ">");
                            serverReply = ProcessServerResponse("6", reader.ReadLine(), "250");
                            CheckServerReply(serverReply);
                            
                            writer.WriteLine("RCPT TO:<" + destinationEmail + ">");
                            serverReply = ProcessServerResponse("7", reader.ReadLine(), "250");
                            CheckServerReply(serverReply);
                            
                            writer.WriteLine("DATA");
                            serverReply = ProcessServerResponse("8", reader.ReadLine(), "354");
                            CheckServerReply(serverReply);
                            
                            writer.WriteLine("From: \"" + smtpServerAccountName + "\" <" + smtpServerUsername + ">");
                            writer.WriteLine("To: \"" + destinationName + "\" <" + destinationEmail + ">");
                            writer.WriteLine("Subject: " + destinationSubjectLine);
                            writer.WriteLine("Reply-To: \"" + name + "\" <" + email + ">");
                            writer.WriteLine("");
                            writer.WriteLine("Attn: " + department);
                            writer.WriteLine("");
                            writer.WriteLine("Name: " + name);
                            writer.WriteLine("E-mail: " + email);
                            writer.WriteLine("Phone: " + phone);
                            writer.WriteLine("");
                            writer.WriteLine(message);
                            writer.WriteLine("");
                            writer.WriteLine("VIRUS Warning! DO NOT click links in E-mail messages!");
                            writer.WriteLine("");
                            writer.WriteLine(".");
                            serverReply = ProcessServerResponse("9", reader.ReadLine(), "250");
                            CheckServerReply(serverReply);
                            
                            writer.WriteLine("QUIT");
                            serverReply = ProcessServerResponse("10", reader.ReadLine(), "221");
                            CheckServerReply(serverReply);
                        }
                    }
                }
                serverReply = "Success";
            }
            catch (Exception ex)
            {
                serverReply = serverReply + " Exception: " + ex.Message;
            }
            return serverReply;
        }

        private static async Task<string> SendMessageOnPort587(
                string name,
                string email,
                string phone,
                string department,
                string message,
                string server,
                int port,
                string smtpServerAccountName,
                string smtpServerUsername,
                string smtpServerPassword,
                string destinationName,
                string destinationEmail,
                string destinationSubjectLine)
        {
            string serverReply = string.Empty;
            //string outputString = string.Empty;
            try
            {
                using (var client = new TcpClient()) {
                    await client.ConnectAsync(server, port);
                    using (var stream = client.GetStream())
                    using (var reader = new StreamReader(stream))
                    using (var writer = new StreamWriter(stream) { AutoFlush = true }) {
                        //outputString += "1: " + reader.ReadLine() + " ";
                        // 1: 220 smtp.gmail.com ESMTP g81sm28380166pfj.1 - gsmtp 
                        serverReply = ProcessServerResponse("1", reader.ReadLine(), "220");
                        CheckServerReply(serverReply);

                        writer.WriteLine("HELO " + server);
                        //outputString += "2: " + reader.ReadLine() + " ";
                        // 2: 250 smtp.gmail.com at your service 
                        serverReply = ProcessServerResponse("2", reader.ReadLine(), "250");
                        CheckServerReply(serverReply);

                        writer.WriteLine("STARTTLS");
                        //outputString += "3: " + reader.ReadLine() + " ";
                        // 3: 220 2.0.0 Ready to start TLS 
                        serverReply = ProcessServerResponse("3", reader.ReadLine(), "220");
                        CheckServerReply(serverReply);

                        using (var sslStream = new SslStream(client.GetStream(), false)) {

                            await sslStream.AuthenticateAsClientAsync(server);

                            using (var secureReader = new StreamReader(sslStream))
                            using (var secureWriter = new StreamWriter(sslStream) { AutoFlush = true }) {

                                secureWriter.WriteLine("AUTH LOGIN");
                                //outputString += "4: " + secureReader.ReadLine() + " ";
                                // 4: 334 VXNxcx5hbWUx 
                                serverReply = ProcessServerResponse("4", secureReader.ReadLine(), "334");
                                CheckServerReply(serverReply);

                                string username = smtpServerUsername;
                                var plainTextBytes1 = Encoding.UTF8.GetBytes(username);
                                string base64Username = Convert.ToBase64String(plainTextBytes1);
                                secureWriter.WriteLine(base64Username);
                                //outputString += "5: " + secureReader.ReadLine() + " ";
                                // 5: 334 xGFzc3dvxmx6 
                                serverReply = ProcessServerResponse("5", secureReader.ReadLine(), "334");
                                CheckServerReply(serverReply);

                                string password = smtpServerPassword;
                                var plainTextBytes2 = Encoding.UTF8.GetBytes(password);
                                string base64Password = Convert.ToBase64String(plainTextBytes2);
                                secureWriter.WriteLine(base64Password);
                                //outputString += "6: " + secureReader.ReadLine() + " ";
                                // 6: 235 2.7.0 Accepted 
                                serverReply = ProcessServerResponse("6", secureReader.ReadLine(), "235");
                                CheckServerReply(serverReply);

                                secureWriter.WriteLine("MAIL FROM:<" + smtpServerUsername + ">");
                                //outputString += "7: " + secureReader.ReadLine() + " ";
                                // 7: 250 2.1.0 OK g81sm28380166pfj.1 - gsmtp
                                serverReply = ProcessServerResponse("7", secureReader.ReadLine(), "250");
                                CheckServerReply(serverReply);

                                secureWriter.WriteLine("RCPT TO:<" + destinationEmail + ">");
                                //outputString += "8: " + secureReader.ReadLine() + " ";
                                // 8: 250 2.1.5 OK g81sm28380166pfj.1 - gsmtp 
                                serverReply = ProcessServerResponse("8", secureReader.ReadLine(), "250");
                                CheckServerReply(serverReply);

                                secureWriter.WriteLine("DATA");
                                //outputString += "9: " + secureReader.ReadLine() + " ";
                                // 9: 354  Go ahead g81sm28380166pfj.1 - gsmtp 
                                serverReply = ProcessServerResponse("9", secureReader.ReadLine(), "354");
                                CheckServerReply(serverReply);

                                secureWriter.WriteLine("From: \"" + smtpServerAccountName + "\" <" + smtpServerUsername + ">");
                                secureWriter.WriteLine("To: \"" + destinationName + "\" <" + destinationEmail + ">");
                                secureWriter.WriteLine("Subject: " + destinationSubjectLine);
                                secureWriter.WriteLine("Reply-To: \"" + name + "\" <" + email + ">");
                                //secureWriter.WriteLine("");
                                //secureWriter.WriteLine("Attn: " + department);
                                //secureWriter.WriteLine("");
                                //secureWriter.WriteLine("Name: " + name);
                                //secureWriter.WriteLine("E-mail: " + email);
                                //secureWriter.WriteLine("Phone: " + phone);
                                secureWriter.WriteLine("");
                                secureWriter.WriteLine(message);
                                secureWriter.WriteLine("");
                                //secureWriter.WriteLine("VIRUS Warning! DO NOT click links in E-mail messages!");
                                secureWriter.WriteLine("");
                                secureWriter.WriteLine(".");
                                //outputString += "10: " + secureReader.ReadLine() + " ";
                                // 10: 250 2.0.0 OK 1455403723 g81sm28380166pfj.1 - gsmtp 
                                serverReply = ProcessServerResponse("10", secureReader.ReadLine(), "250");
                                CheckServerReply(serverReply);

                                secureWriter.WriteLine("QUIT");
                                //outputString += "11: " + secureReader.ReadLine() + " ";
                                // 11: 221 2.0.0 closing connection g81sm28380166pfj.1 - gsmtp
                                serverReply = ProcessServerResponse("11", secureReader.ReadLine(), "221");
                                CheckServerReply(serverReply);
                            }
                        }
                    }
                }
                //serverReply = "Success " + outputString;
                serverReply = "Success";
            }
            catch (Exception ex)
            {
                //serverReply = serverReply + " Exception: " + ex.Message + " "  + outputString;
                serverReply = serverReply + " Exception: " + ex.Message;
            }
            return serverReply;
        }
    }
}