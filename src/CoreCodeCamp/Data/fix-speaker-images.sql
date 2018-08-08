UPDATE dbo.Speakers
   SET ImageUrl = 'https://wilderminds.blob.core.windows.net/acc' + LOWER(REPLACE(REPLACE(ImageUrl, '//', '/'), '\\', '/'))
 WHERE SUBSTRING(ImageUrl,1,1) = '/' OR SUBSTRING(ImageUrl,1,1) = '\\'