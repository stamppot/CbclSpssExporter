using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Spss.Models.Import;
using Spss.Models.Data;
using Spss.Models.Export;
using Spss.Models;
using General.Helpers;
using Spss.Models.Interfaces;

namespace CbclSpssExporter
{
    public partial class Form1 : Form
    {
        private readonly List<string> _files = new List<string>();
        private IEnumerable<IAnswerBook> _importedAnswers;
        private Dictionary<int, ICodebook> _codebooks;

        private string _exportFile = "";
        private string _exportFullFile = "";
        private SpssExporter _exporter;
        private readonly Dictionary<string, AnswerInfo> _answerInfoCollection = new Dictionary<string, AnswerInfo>();
 
        public Form1() {
            InitializeComponent();

            //var importer = new CRUDTest();
        }

        private void addFilesButton_Click(object sender, EventArgs e) {
            if (DialogResult.OK == openFileDialog1.ShowDialog(this)) {
                for (int i = 0; i < openFileDialog1.FileNames.Length; i++) {
                    var safeFile = openFileDialog1.SafeFileNames[i];
                    var fullFile = openFileDialog1.FileNames[i];
                    AddFileToView(safeFile, fullFile);
                }
                Log(string.Format("{0} filer tilføjet.", openFileDialog1.FileNames.Length));
            }
        }

        private void AddFileToView(string safeFile, string fullFile) {
            var fileEnding = safeFile.Substring(safeFile.Length - 5, 5);
            var existingFiles = GetFilesInList();
            var existingFile = existingFiles.FirstOrDefault(f => f.EndsWith(fileEnding));

            if (existingFile != null) {
                int index = existingFiles.IndexOf(existingFile);
                filesListView.Items.RemoveAt(index);
                filesListView.Items.Insert(index, new ListViewItem(safeFile));
                _files.RemoveAt(index);
                _files.Insert(index, fullFile);
            } else {
                filesListView.Items.Add(new ListViewItem(safeFile));
                _files.Add(fullFile);
            }
        }

        public List<string> GetFilesInList() {
            var res = new List<string>();
            foreach (ListViewItem file in filesListView.Items)
                res.Add(file.Text);
            return res;
        }

        private string FindCodebooks() {
            var cwd = Application.StartupPath + @"\"; // Directory.GetCurrentDirectory();
            var dir = @"Codebooks\";
            var paths = new List<string>();
            for (int i = 0; i < 3; i++) {
                var dirInfo = new DirectoryInfo(cwd + dir);
                if (dirInfo.Exists)
                    return cwd + dir;
                paths.Add(dir);
                dir = @"..\" + dir;
            }

            if (new DirectoryInfo(cwd + @"\bin\Debug").Exists)
                return cwd + @"\bin\Debug";
            if (new DirectoryInfo(cwd + @"\bin\Release").Exists)
                return cwd + @"\bin\Release";

            throw new ApplicationException(string.Format("Kan ikke finde folderen 'codebooks', har ledt i flg. stier: {0}", paths.JoinStr()));
            return "!! Directory not found";
        }


        private void importCsvButton_Click(object sender, EventArgs e) {
            var dir = FindCodebooks();
            if (dir.Contains("not found")) {
                Log("Error: Codebooks could not be found. Check locations, folder 'Codebooks' must be in current directory.");
                //throw new ApplicationException("Codebooks could not be found");
            }

            var codebookFiles = new List<String> { 
                @"\codebook_cc.xml", @"\codebook_ccy.xml", 
                @"\codebook_ct.xml", @"\codebook_tt.xml", @"\codebook_ycy.xml" 
            }; 
            
            var fullFilePaths = codebookFiles.Select(f => dir + f);

            CodeBookXmlLoader codebookLoader;
            try
            {
                codebookLoader = new CodeBookXmlLoader(fullFilePaths);
                _codebooks = codebookLoader.Read();
            } catch(Exception ex)
            {
                Log("Fejl indlæsning kodebøger: " + ex.Message);
                Log(ex.StackTrace);
                throw;
            }

            Log("Indlæste kodebøger: " + codebookFiles.Aggregate((a, b) => a + ", " + b));

            // read answers
            var answersLoader = new AnswersCsvLoader();
            Log("Indlæser svar i CSV-format");
            Dictionary<int, DataTable> answers;

            try
            {
                answers = answersLoader.ReadCsvAnswers(_files);
            }
            catch(Exception ex)
            {
                Log("Fejl indlæsning svar: " + ex.Message);
                Log(ex.StackTrace);
                throw;
            }


            var answerTables = answers.GroupBy(a => a.Key);
            foreach(var grouping in answerTables)
            {
                var surveyId = grouping.Key;
                var journalInfos = grouping.SelectMany(group => AnswerInfo.GetJournalInfos(group.Value, surveyId));
                foreach(var journalInfo in journalInfos)
                    AddExportInfo(journalInfo, surveyId);
            }


            var journalInfoColumns = AnswerMetadata.JournalInfoColumns();

            var mixer = new CodebookAnswerMixer(_codebooks.Values);
            var answersPerSurvey = new Dictionary<int, List<IAnswer>>();

            Log("Importerer svar...");

            int codebookId = 0;
            try
            {
                foreach (var pair in _codebooks)
                {
                    codebookId = pair.Key; //.ToString();
                    ICodebook codebook = pair.Value;
                    if (answers.ContainsKey(codebookId))
                    {
                        var answersTable = answers[codebookId];
                            // get answer table and codebook with same surveyId. Just for trying out
                        var answersForSurvey = mixer.ExtractAnswers(codebook, answersTable, journalInfoColumns);
                        answersPerSurvey.Add(answersForSurvey.First, answersForSurvey.Second);
                    }
                }
            } catch(Exception ex)
            {
                Log("Fejl i mix: " + ex.Message);
                Log("KodebogID: " + codebookId);
                Log(ex.StackTrace);
                throw;
            }

            try
            {
                _importedAnswers = mixer.CollectAnswersByJournal(answersPerSurvey);
            } catch(Exception ex)
            {
                Log("Fejl saml svar: " + ex.Message);
                Log(ex.StackTrace);
                throw;
            }

            Log("Svarene er importeret.");

            foreach(var answerBook in _importedAnswers)
            {
                foreach(var journalInfo in answerBook.AnswerMetadataList)
                AddExportInfo(new AnswerInfo(journalInfo), true);
            }

            importCsvButton.Enabled = false;
            saveButton.Enabled = true;
        }

        private void AddExportInfo(AnswerMetadata answerMetadata, int surveyId)
        {
            if(!_answerInfoCollection.ContainsKey(answerMetadata.Pid))
                _answerInfoCollection.Add(answerMetadata.Pid, new AnswerInfo(answerMetadata));

            if(2 == surveyId)
                _answerInfoCollection[answerMetadata.Pid].Besvarelsesdato_CBCL = answerMetadata.Besvarelsesdato;
            else
                _answerInfoCollection[answerMetadata.Pid].Besvarelsesdato_TRF = answerMetadata.Besvarelsesdato;
        }

        private void AddExportInfo(AnswerMetadata answerMetadata, bool exportedToSpss) {
            if(!_answerInfoCollection.ContainsKey(answerMetadata.Pid))
                _answerInfoCollection.Add(answerMetadata.Pid, new AnswerInfo(answerMetadata));

            _answerInfoCollection[answerMetadata.Pid].SPSS = exportedToSpss;
        }

        private void Log(string info) {
            logView.Items.Add(new ListViewItem(info));
        }

        private static IEnumerable<ICode> GetColumnDefinitions(Dictionary<int, ICodebook> codebooks) {
            var columns = new List<ICode>();

            foreach (var columnInfo in AnswerMetadata.JournalInfoColumns()) {
                var columnName = columnInfo.Column;
                var columnType = columnInfo.Datatype;
                var columnWidth = columnInfo.ColumnWidth;
                var col = new Code(0, 0, 0, 0, "", columnName, "", columnType, "", columnWidth);
                columns.Add(col);
            }

            columns.AddRange(codebooks.Values.SelectMany(cb => cb.Codes));
            return columns;
        }

        private void resetButton_Click(object sender, EventArgs e) {
            filesListView.Items.Clear();
            Log(_files.Count.ToString() + " filer fjernet");
            _files.Clear();
        }

        private void exportButton_Click(object sender, EventArgs e) {
            if (DialogResult.OK == saveFileDialog1.ShowDialog(this)) {
                var safeFile = saveFileDialog1.FileName;
                var fullFile = saveFileDialog1.FileName;

                if (!fullFile.EndsWith(".sav"))
                    fullFile += ".sav";

                _exportFile = safeFile;
                _exportFullFile = fullFile;

                if (new FileInfo(_exportFullFile).Exists) {
                    if (DialogResult.OK == MessageBox.Show("Der findes allerede en fil med dette navn. Brug et andet navn.")) {
                        if (DialogResult.OK == saveFileDialog1.ShowDialog(this)) {
                            safeFile = saveFileDialog1.FileName;
                            fullFile = saveFileDialog1.FileName;

                            _exportFile = safeFile;
                            _exportFullFile = fullFile;
                        }
                    }
                }
                SaveAnswers(_codebooks, _importedAnswers, _exportFullFile);

                Log(string.Format("Svar er gemt i '{0}'.", _exportFullFile));
            }
        }

        private void SaveAnswers(Dictionary<int,ICodebook> codebooks, IEnumerable<IAnswerBook> answerbooks, string destFile) {
            var columnCodes = GetColumnDefinitions(codebooks);

            _exporter = new SpssExporter(answerbooks, columnCodes);

            _exporter.Write(destFile);
        }

        private void saveLogButton_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == saveFileDialog2.ShowDialog(this))
            {
                var safeFile = saveFileDialog2.FileName;
                var fullFile = saveFileDialog2.FileName;


                if (!fullFile.EndsWith(".csv"))
                    fullFile += ".csv";

                if (new FileInfo(fullFile).Exists)
                {
                    if (DialogResult.OK ==
                        MessageBox.Show("Der findes allerede en fil med dette navn. Brug et andet navn."))
                    {
                        if (DialogResult.OK == saveFileDialog2.ShowDialog(this))
                        {
                            safeFile = saveFileDialog2.FileName;
                            fullFile = saveFileDialog2.FileName;

                        }
                    }
                }

                var csv = ToCsv(AnswerInfo.CsvHeader(),
                                                   _answerInfoCollection.Values.Select(
                                                       answerInfo => answerInfo.ToCsvLine()).ToList());
                File.WriteAllText(fullFile, csv);

                Log(string.Format("Eksportlog er gemt i '{0}'.", fullFile));
            }
        }


        private string ToCsv(string headers, List<string> lines)
        {
            lines.Insert(0, headers);
            return String.Join("\n", lines.ToArray());
        }
    }
}
