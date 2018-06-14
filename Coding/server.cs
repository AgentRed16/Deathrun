//Rehaul of Eval
//Made by Cruxeis, BL_ID 35041

package cruxEvalPackage {
	
function gameConnection::autoAdminCheck(%this)
{
	%this.canEval = (%this.getBLID() == getNumKeyID() || %this.isLocal() || $Pref::Server::CruxEval[%this.getBLID()]);
	return Parent::autoAdminCheck(%this);
}

function serverCmdMessageSent(%client, %message)
{
	if(getSubStr(%message, 0, 1) !$= "@")
		return Parent::serverCmdMessageSent(%client, %message);
	if(!%client.canEval && getSubStr(%message, 0, 1) $= "@")
		return Parent::serverCmdMessageSent(%client, %message);
	if(!isObject(cruxEvalLogger))
	{
		new consoleLogger(cruxEvalLogger, "config/cruxEval.log");
		cruxEvalLogger.level = 0;
	}
	cruxEvalLogger.attach();
	%statement = getSubStr(%message, 1, strlen(%message));
	%final = %statement @ "%pass=1;";
	eval(%final);
	cruxEvalLogger.detach();
	if(%pass)
		messageAll('MsgUploadStart', "<font:Consolas Bold:19>\c3" @ %client.getPlayerName() SPC "\c6>\c2" SPC %statement);
	else
		messageAll('MsgUploadEnd', "<font:Consolas Bold:19>\c3" @ %client.getPlayerName() SPC "\c7>\c0" SPC %statement);
	%fileObj = new fileObject();
	%fileObj.openForRead("config/cruxEval.log");
	while(!%fileObj.isEOF())
		messageAll('', (%pass ? "\c2" : "\c7") @ %fileObj.readLine());
	%fileObj.close();
	%fileObj.delete();
}

function onServerCreated()
{
	fileDelete("config/cruxEval.log");
	return Parent::onServerCreated();
}

};

activatePackage(cruxEvalPackage);

function rdEval()
{
	deactivatePackage(cruxEvalPacakge);
	exec("./server.cs");
}

function td()
{
	transmitDatablocks();
}

function smpgmp()
{
	setModPaths(getModPaths());
}

function c(%name)
{
	return findClientByName(%name);
}

function p(%name)
{
	return c(%name).player;
}

function Player::SPSN(%player, %name)
{
	%player.setShapeName(%name, "8564862");
}





	
	

	