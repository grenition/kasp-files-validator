namespace Project.Services.FileStreamer
{
    public class FileStreamer : IFileStreamer
    {
        private string? _originalFilePath;
        private string? _tempFilePath;
        private StreamReader? _reader;
        private StreamWriter? _writer;
        
        private string? _lineBuffer;
        private bool _eofReached;
        
        private bool _isOpen;

        public void OpenFile(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("File is not found.", filePath);

            _originalFilePath = filePath;
            
            var directory = Path.GetDirectoryName(filePath)!;
            var fileNameWithoutExt = Path.GetFileNameWithoutExtension(filePath);
            var extension = Path.GetExtension(filePath);
            var guid = Guid.NewGuid().ToString("N");

            _tempFilePath = Path.Combine(directory, $"{fileNameWithoutExt}.{guid}{extension}");

            _reader = new StreamReader(filePath);
            _writer = new StreamWriter(_tempFilePath);

            _lineBuffer = null;
            _eofReached = false;
            _isOpen = true;
        }

        public bool GetLine(out string? line)
        {
            if (!_isOpen)
                throw new InvalidOperationException("File is not opened.");

            line = string.Empty;
            
            FlushBufferedLine();

            if (_eofReached || _reader!.EndOfStream)
            {
                _eofReached = true;
                return false;
            }

            var readLine = _reader.ReadLine();
            if (readLine == null)
            {
                _eofReached = true;
                return false;
            }

            _lineBuffer = readLine;
            line = _lineBuffer;
            return true;
        }

        public void ModifyPreviousLine(string? newValue)
        {
            if (!_isOpen)
                throw new InvalidOperationException("File is not opened.");

            if (_lineBuffer == null)
                throw new InvalidOperationException("Nothing to change — line was not readed.");

            _lineBuffer = newValue;
        }

        public void CloseFile()
        {
            if (!_isOpen)
                throw new InvalidOperationException("File is not opened.");

            try
            {
                FlushBufferedLine();

                if (!_eofReached)
                {
                    string? line;
                    while ((line = _reader!.ReadLine()) != null)
                    {
                        _writer!.WriteLine(line);
                    }
                }
            }
            finally
            {
                _reader?.Close();
                _writer?.Close();
                
                _reader = null;
                _writer = null;
                _isOpen = false;
            }

            File.Delete(_originalFilePath!);
            File.Move(_tempFilePath!, _originalFilePath!);
        }
        
        private void FlushBufferedLine()
        {
            if (_lineBuffer != null)
            {
                if (_reader!.EndOfStream)
                    _writer!.Write(_lineBuffer);
                else
                    _writer!.WriteLine(_lineBuffer);

                _lineBuffer = null;
            }
        }
    }
}
