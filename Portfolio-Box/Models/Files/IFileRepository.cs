using System.Collections.Generic;

namespace Portfolio_Box.Models.Files;

public interface IFileRepository
{
	public IEnumerable<File> AllFiles { get; }
	public File? GetFileById(int id);
	public File? GetFileByDownloadUri(string downloadUri);
	public void SaveFile(File file);
	public void DeleteFile(File file);
	public IEnumerable<File> GetFilesByPath(HashSet<string> paths);
	public void SaveFiles(File[] files);
	public void UpdateFile(File file);
}