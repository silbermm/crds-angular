USE [MinistryPlatform]
GO

declare @base varchar(50) = 'www.crossroads.net';
-- integration background check url = https://www.employmentreports.net/Application/Subject/DisclosureForm
declare @backgroundurl varchar(255) = N'https://www.employmentreports.net/Application/Subject/DisclosureForm';


BEGIN
UPDATE [dbo].[dp_Communications]
   SET [Body] = N'Hello there, <br /><br /> Thank you for your continued interest in serving. There''s additional information we would like to gather from you in order to help us learn more about you. <br /> <br /><a href=''https://' + @base +'/volunteer-application/kids-club'' target=''_self''>Please fill out the Kids'' Club application</a>. <br /> <br /> If you are filling out this application on behalf of your child who''s between the ages of 10 and 13, invite your child to join you and complete the application together. <br /><br /> Thank you,'
 WHERE Communication_ID = 13805;


UPDATE [dbo].[dp_Communications]
   SET [Body] = N'Hello there, <br /><br /> Thank you for your continued interest in serving. As part of our policy, we are requesting that you fill out a background check.<div><br /></div><div><a href=''' + @backgroundurl + '?cra=525VER&username=CrossroadsPDF&application=CrossroadsDisclosure.pdf&externalId=[Contact_ID]&redirectUrl=https%3A%2F%2' + @base + '%2F%23%2Fbackgroundcheck-thanks%2F'' target=''_self''>Please fill out the background check and required information</a>.<br /><br /> Please note, our background check is handled by <a href=''http://veridataservices.com'' target=''_self''>VeriData</a>  This information will not be public or used in crossroads.net.<br /><br /> Thank you,</div>'
 WHERE  Communication_ID = 11269;
END