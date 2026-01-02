using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Portfolio_Box.Extensions;
using Portfolio_Box.Models.Users;

namespace Portfolio_Box.Models.Files;

public class FileRepository : IFileRepository
{
	private readonly AppDBContext _appDBContext;
	private readonly RemoteFileAvailabilityChecker _checker;
	private readonly IFileFactory _sharedFileFactory;
	private readonly User _user;
	public readonly string MediaBasePath;

	public FileRepository(AppDBContext dbContext, User user, IFileFactory fileFactory, IConfiguration configuration, RemoteFileAvailabilityChecker checker)
	{
		MediaBasePath = configuration.GetMediaBasePath();

		_appDBContext = dbContext;
		_user = user;
		_sharedFileFactory = fileFactory;
		_checker = checker;
	}

	public bool IsRemoteAvailable => _checker.IsAvailable;

	public IEnumerable<File> AllFiles
		=> from f in _appDBContext.Files
			where f.UserId == _user.Id
			orderby f.UploadedOn descending
			select f;

	public IEnumerable<File> GetFilesByPath(HashSet<string> paths)
	{
		return from f in _appDBContext.Files
			where f.UserId == _user.Id && paths.Contains(f.DiskPath)
			orderby f.UploadedOn descending
			select f;
	}

	public File? GetFileById(int id)
	{
		return (from f in _appDBContext.Files
				where f.Id == id && f.UserId == _user.Id
				select f)
			.Include(c => c.Link)
			.FirstOrDefault();
	}

	public File? GetFileByDownloadUri(string downloadUri)
	{
		return (from f in _appDBContext.Files
				where f.Link != null && f.Link.DownloadUri == downloadUri && f.Link.Expiration > DateTime.Now
				select f)
			.FirstOrDefault();
	}

	public void SaveFile(File file)
	{
		_appDBContext.Files.Add(file);
		_appDBContext.SaveChanges();
	}

	public void SaveFiles(File[] files)
	{
		_appDBContext.Files.AddRange(files);
		_appDBContext.SaveChanges();
	}

	public void UpdateFile(File file)
	{
		_appDBContext.Files.Update(file);
		_appDBContext.SaveChanges();
	}

	public void DeleteFile(File file)
	{
		_appDBContext.Files.Remove(file);
		_appDBContext.SaveChanges();
		if (file.Remote)
			return;
		_ = _sharedFileFactory.DeleteFileAsync(file);
	}
}