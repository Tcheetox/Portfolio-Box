using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Portfolio_Box.Models.Files;
using Portfolio_Box.Models.Users;

namespace Portfolio_Box.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RemoteFileController : ControllerBase
{
	private readonly IFileFactory _fileFactory;
	private readonly IFileRepository _fileRepository;
	private readonly User _user;

	public RemoteFileController(
		IFileRepository fileRepository,
		IFileFactory fileFactory,
		IConfiguration configuration,
		User user)
		: base(configuration)
	{
		_fileRepository = fileRepository;
		_fileFactory = fileFactory;
		_user = user;
	}

	private void ThrowIfNotAuthorizedAdmin()
	{
		if (_user is AdminUser)
			return;
		throw new UnauthorizedAccessException("Remote file controller is only for KRE admin!");
	}

	[HttpPost]
	public async Task<IActionResult> Create(IReadOnlyList<RemoteFileDto> files)
	{
		ThrowIfNotAuthorizedAdmin();

		var filesPath = files.Select(f => f.Path).ToHashSet();
		var existingFiles = _fileRepository.GetFilesByPath(filesPath).Select(f => f.DiskPath).ToList();
		var missingFiles = files.Where(f => !existingFiles.Contains(f.Path));
		var newFiles = await _fileFactory.CreateFilesAsync(missingFiles, HttpContext.RequestAborted);

		if (newFiles.Length > 0)
		{
			_fileRepository.SaveFiles(newFiles);
			return Created();
		}

		return Ok();
	}

	[HttpPut]
	public IActionResult Update(UpdatedRemoteFileDto file)
	{
		ThrowIfNotAuthorizedAdmin();

		var lookupPath = string.IsNullOrWhiteSpace(file.PreviousPath) ? file.Path : file.PreviousPath;
		var existingFile = _fileRepository.GetFilesByPath([lookupPath]).FirstOrDefault();
		if (existingFile is not null)
		{
			existingFile.DiskPath = file.Path;
			existingFile.Length = file.Length;
			existingFile.OriginalName = WebUtility.HtmlEncode(Path.GetFileName(file.Path));
			_fileRepository.UpdateFile(existingFile);
		}

		return Ok();
	}

	[HttpDelete("{filePath}")]
	public IActionResult Delete(string filePath)
	{
		ThrowIfNotAuthorizedAdmin();

		filePath = WebUtility.UrlDecode(filePath);
		var target = _fileRepository
			.GetFilesByPath([filePath])
			.FirstOrDefault();
		if (target is not null)
			_fileRepository.DeleteFile(target);
		return Ok();
	}
}