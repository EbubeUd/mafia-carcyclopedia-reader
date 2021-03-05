using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CarcyclopediaReader
{
    public static class BinReaderService
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="binPath">Path to the bin file</param>
        /// <param name="jsonPath">Folder Where the json file should be created</param>
        /// <param name="shouldExportAsJson">If true, the bin file will be exported as json and saved in the specified path</param>
        public static void  ReadBinFileAsync(string binPath, string jsonPath = "", bool shouldExportAsJson = false)
        {



            BinaryReader reader = new BinaryReader(File.Open(binPath, FileMode.Open));

            VehicleModel[] vehicles = new VehicleModel[80];
            VehicleModel model = new VehicleModel();
            char[] chars = reader.ReadChars(200);
            for (int m = 0; m < 80; m++)
            {

                try
                {

                    ReadEmptyBytes(reader);
                    model = new VehicleModel();
                    #region Association
                    model.association.vehModelName = ReadNullTerminatedString(reader);
                    ReadEmptyBytes(reader);
                    model.association.vehWreckFileName = ReadNullTerminatedString(reader);
                    ReadEmptyBytes(reader);
                    model.association.vehShadFileName = ReadNullTerminatedString(reader);
                    ReadEmptyBytes(reader);
                    model.association.vehIngameName = ReadNullTerminatedString(reader);
                    ReadEmptyBytes(reader);
                    #endregion

                    #region Categorization
                    model.categorization.vehRaceCategory = ReadInt32(reader);
                    model.categorization.vehClassID = ReadInt32(reader);
                    model.categorization.unknown2 = ReadInt32(reader);
                    model.categorization.vehStealMask = ReadInt32(reader);
                    model.categorization.unknown3 = ReadInt32(reader);
                    model.categorization.unknown4 = ReadInt32(reader);
                    model.categorization.vehGarageMask = ReadInt32(reader);
                    model.categorization.vehGroupID = ReadInt32(reader);
                    model.categorization.vehColorNum = ReadInt32(reader);
                    model.categorization.vehEncyMask = ReadInt32(reader);
                    #endregion


                    #region Colors
                    model.colors.vehCol1 = ReadInt32(reader);
                    model.colors.vehCol2 = ReadInt32(reader);
                    model.colors.vehCol3 = ReadInt32(reader);
                    model.colors.vehCol4 = ReadInt32(reader);
                    model.colors.vehCol5 = ReadInt32(reader);
                    #endregion

                    #region Others
                    model.other.vehDescription = ReadInt32(reader);
                    #endregion

                    vehicles[m] = model;
                    reader.ReadBytes(6);
                    
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }


            if (shouldExportAsJson) ExportAsJson(vehicles, jsonPath);







        }



        static bool ReadBool(this System.IO.BinaryReader stream)
        {
            return BitConverter.ToBoolean(stream.ReadBytes(1), 0);
        }

        static int ReadInt32(this System.IO.BinaryReader stream)
        {
            return BitConverter.ToInt32(stream.ReadBytes(4), 0);
        }

        static float ReadFloat(this System.IO.BinaryReader stream)
        {
            return BitConverter.ToSingle(stream.ReadBytes(4), 0);
        }

        static string ReadNullTerminatedString(this System.IO.BinaryReader stream)
        {
            string str = "";
            char ch;
            try
            {
                while ((int)(ch = stream.ReadChar()) != 0)
                {

                    str = str + ch;
                }
            }
            catch (Exception ex)
            {

            }


            return str;
        }

        static string ReadLengthCharacterString(this System.IO.BinaryReader stream, int maxLength)
        {
            string str = ReadNullTerminatedString(stream);
            int stringCount = str.Length;
            //Check if the length is greater than the max length
            if (stringCount > maxLength)
            {
                str = str.Substring(0, maxLength);
                int newCount = str.Length;
                int diff = stringCount - newCount;
                stream.BaseStream.Position -= (diff + 1);
            }
            else
            {
                int posSkip = maxLength - stringCount;
                if (posSkip > 0) stream.ReadBytes(posSkip - 1);
            }

            return str;
        }

        static string ReadStringInFixedBytes(this System.IO.BinaryReader stream, int byteLength)
        {
            string str;
            str = ReadNullTerminatedString(stream);
            int byteCount = System.Text.ASCIIEncoding.ASCII.GetByteCount(str);
            int extraBytes = byteLength - byteCount;
            if (extraBytes > 0) stream.ReadBytes(extraBytes);
            return str;
        }

        static void ReadEmptyBytes(this System.IO.BinaryReader stream)
        {
            while (true)
            {
                byte b = stream.ReadByte();
                if (b != 0)
                {
                    stream.BaseStream.Position--;
                    break;
                }
            }


        }
        static void ExportAsJson<T>(T model, string pathToJson)
        {
            Console.WriteLine("Exporting Json...");

            string jsonString = JsonConvert.SerializeObject(model);

            // Check if file already exists. If yes, delete it.     
            if (File.Exists(@pathToJson))
            {
                File.Delete(pathToJson);
            }

            using (FileStream fs = File.Create(pathToJson))
            {
                // Add some text to file    
                Byte[] title = new UTF8Encoding(true).GetBytes(JsonConvert.SerializeObject(model, Formatting.Indented));
                fs.Write(title, 0, title.Length);

            }

            Console.WriteLine($"Exported to {pathToJson}");
        }
    }
}
