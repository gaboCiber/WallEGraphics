using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WallE.Graphics
{
    public class Depurate
    {
        public bool IsThereAnyError {get; private set;}

        public string Error;

        public List<string> ImportFiles;

        public string Output;

        public Depurate(string code)
        {
            ImportFiles = new List<string>();
            Output = DepurateCode(code);
        }

        private string DepurateCode(string input)
        {
            StringBuilder codeD = new StringBuilder();
            bool importStament = false;
            bool noMoreImport = false;
            
            for (int i = 0, line = 1, col = 1; i < input.Length; i++, col++)
            {
                
                if(input[i] == ' ')
                {
                    codeD.Append(input[i]);
                    continue;
                }

                if(input[i] == '\n')
                {
                    codeD.Append(input[i]);
                    line++;
                    col=0;
                    continue;
                }

                if(input[i] == ';')
                {
                    if(noMoreImport)
                        codeD.Append(input[i]);
                    
                    continue;
                }
                
                if(importStament)
                {
                    if(input[i] != '\"')
                    {
                        IsThereAnyError = true;
                        return Error = $"Syntax Error: Missing character double-quotes ' \" ' after import keyword [ln {line}, Col {col}]";

                    }

                    StringBuilder path = new StringBuilder();
                    //path.Append('\"');
                    i++;

                    while (true)
                    {
                        if (i == input.Length)
                        {
                            IsThereAnyError = true;
                            return Error = $"Syntax Error: Missing character double-quotes ' \" ' [ln {line}, Col {col}]";
                        }

                        if (input[i] == '\n')
                        {
                            line++;
                            col = 1;
                            continue;
                        }

                        if (input[i] == '"')
                        {
                            break;
                        }

                        path.Append(input[i]);

                        i++;
                        col++;
                    }

                    ImportFiles.Add(path.ToString());
                    importStament = noMoreImport = false;
                    continue;
                }

                if(char.IsLetter(input[i]))
                {
                    StringBuilder word = new StringBuilder();

                    while (i != input.Length && char.IsLetterOrDigit(input[i]))
                    {
                        word.Append(input[i]);
                        i++;
                        col++;
                    }

                    if(word.ToString() == "import")
                    {
                        if(noMoreImport)
                        {
                            IsThereAnyError = true;
                            return Error = $"Syntax Error: Import stament must be used at the beginig of the file [ln {line}, Col {col}]";
                        }

                        importStament = true;
                    }
                    else
                    {
                        noMoreImport = true;
                        codeD.Append(word);
                    }

                    i--;
                }

                else if(input[i] == '/' && i < input.Length - 1 && input[i+1] == '/')
                {
                    while (i < input.Length && input[i] != '\n')
                    {
                        i++;
                    }

                    line++;
                    col = 0;
                    continue;
                }

                else
                {
                    codeD.Append(input[i]);
                    noMoreImport = true;
                }


            }

            for (int i = 0; i < codeD.Length; i++)
            {
                if(codeD[i] == ' ' || codeD[i] == '\n')
                {
                    codeD.Remove(i,1);
                    i--;
                }
                else
                    break;
            }

            return codeD.ToString();
        }

        private string DepurateCodeOLD(string code)
        {
            StringBuilder codeDepurated = new StringBuilder();
            bool start = false;

            for (int i = 0, line = 1, col = 1; i < code.Length; i++, col++)
            {
                if(code[i] == '\n')
                {
                    line++;
                    col = 0;
                }

                if(char.IsLetter(code[i]))
                {
                    StringBuilder word = new StringBuilder();

                    while (true)
                    {
                        if( i == code.Length)
                        {
                            codeDepurated.Append(word);
                            return codeDepurated.ToString();
                        }

                        if(code[i] == ' ')
                            break;

                        word.Append(code[i]);

                        i++;
                        col++;
                    }

                    if(word.ToString() == "import")
                    {
                        word.Append("NNNNNNNNNNNNNN");
                        if(start)
                        {
                            IsThereAnyError = true;
                            return $"Syntax Error: Import stament must be used at the beginig of the file [ln {line}, Col {col}]";
                        }
                        
                        StringBuilder comilla = new StringBuilder();
                        bool comillaSt = false;
                        
                        while(true)
                        {
                            if(i == code.Length)
                            {
                                IsThereAnyError = true;
                                return $"Syntax Error at line {line} in column {col}: Missing character '\"'";
                            }
                            
                            if(code[i] == ' ')
                            {
                                i++;
                                col++;
                                continue;
                            }

                            if(!comillaSt && code[i] != '\"')
                            {
                                IsThereAnyError = true;
                                return $"Syntax Error at line {line} in column {col}: Missing character '\"' after import stament";
                            }

                            comillaSt = true;
                            comilla.Append(code[i]);

                            if(comillaSt && code[i] == '\"')
                            {
                                ImportFiles.Add(comilla.ToString());
                                break;
                            }

                            i++;
                            col++;
                        }

                    }
                    else
                    {
                        start = true;
                        codeDepurated.Append(word);
                        codeDepurated.Append(code[i]);
                    }
                }
                else
                {
                    codeDepurated.Append(code[i]);
                }


            }

            return codeDepurated.ToString();
        } 
    }
}