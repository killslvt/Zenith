--!nonstrict

--[==[

caesar admin v1.1
- https://github.com/byteveil

]==]

if type(_G.caesar) == 'table' then
	_G.caesar.cleanup()
end

local players = game:GetService('Players')
local uis = game:GetService('UserInputService')
local networkclient = game:GetService('NetworkClient')
local startergui = game:GetService('StarterGui')
local lighting = game:GetService('Lighting')
local stepped = game:GetService('RunService').Stepped
local clientreplicator = networkclient:FindFirstChildWhichIsA('ClientReplicator')
local localplayer = clientreplicator:GetPlayer()
local spawn = task.spawn
local insert = table.insert
local studiomode = game:GetService('RunService'):IsStudio()
local scriptmode = typeof(script) == 'Instance'
local playergui = scriptmode and localplayer:FindFirstChildWhichIsA('PlayerGui') or game:GetService('CoreGui')
local caesar

local function split(str, delim)
	local broken = {}
	if type(delim) ~= 'string' then
		delim = ','
	end
	for w in str:gmatch('[^' .. delim .. ']+') do
		insert(broken, w)
	end
	return broken
end

local function find(t, v)
	if type(t) == 'table' then
		for _, x in pairs(t) do
			if x == v then
				return true
			end
		end
	end
	return false
end

local ScreenGui = Instance.new("ScreenGui")
ScreenGui.Name = "ScreenGui"
ScreenGui.DisplayOrder = 999999
ScreenGui.ZIndexBehavior = Enum.ZIndexBehavior.Sibling

local ExecutionFrame = Instance.new("Frame")
ExecutionFrame.Name = "ExecutionFrame"
ExecutionFrame.AnchorPoint = Vector2.new(0.5, 0)
ExecutionFrame.BackgroundColor3 = Color3.fromRGB(25, 25, 25)
ExecutionFrame.BorderColor3 = Color3.fromRGB(51, 51, 51)
ExecutionFrame.BorderSizePixel = 0
ExecutionFrame.Position = UDim2.new(0.5, 0, 0, 15)
ExecutionFrame.Size = UDim2.new(0.25, 0, 0, 30)

local UIStroke = Instance.new("UIStroke")
UIStroke.Name = "UIStroke"
UIStroke.Color = Color3.fromRGB(51, 51, 51)
UIStroke.Thickness = 2
UIStroke.Parent = ExecutionFrame

local ExecutionField = Instance.new("TextBox")
ExecutionField.Name = "ExecutionField"
ExecutionField.CursorPosition = -1
ExecutionField.FontFace = Font.new("rbxasset://fonts/families/RobotoMono.json")
ExecutionField.PlaceholderText = "command [arg1] [arg2]"
ExecutionField.Text = ""
ExecutionField.TextColor3 = Color3.fromRGB(255, 255, 255)
ExecutionField.TextScaled = true
ExecutionField.TextSize = 14
ExecutionField.TextWrapped = true
ExecutionField.BackgroundColor3 = Color3.fromRGB(255, 255, 255)
ExecutionField.BackgroundTransparency = 1
ExecutionField.BorderColor3 = Color3.fromRGB(0, 0, 0)
ExecutionField.BorderSizePixel = 0
ExecutionField.Size = UDim2.new(1, 0, 1, 0)

local UITextSizeConstraint = Instance.new("UITextSizeConstraint")
UITextSizeConstraint.Name = "UITextSizeConstraint"
UITextSizeConstraint.MaxTextSize = 18
UITextSizeConstraint.Parent = ExecutionField

ExecutionField.Parent = ExecutionFrame

ExecutionFrame.Parent = ScreenGui

local CommandFrame = Instance.new("Frame")
CommandFrame.Name = "CommandFrame"
CommandFrame.AnchorPoint = Vector2.new(1, 0)
CommandFrame.BackgroundColor3 = Color3.fromRGB(25, 25, 25)
CommandFrame.BorderColor3 = Color3.fromRGB(51, 51, 51)
CommandFrame.BorderSizePixel = 0
CommandFrame.Position = UDim2.new(1, -15, 0, 15)
CommandFrame.Size = UDim2.new(0.3, 0, 0.2, 0)

local UIStroke1 = Instance.new("UIStroke")
UIStroke1.Name = "UIStroke"
UIStroke1.Color = Color3.fromRGB(51, 51, 51)
UIStroke1.Thickness = 2
UIStroke1.Parent = CommandFrame

local CommandList = Instance.new("ScrollingFrame")
CommandList.Name = "CommandList"
CommandList.ScrollBarImageColor3 = Color3.fromRGB(204, 204, 204)
CommandList.Active = true
CommandList.AnchorPoint = Vector2.new(0.5, 0)
CommandList.BackgroundColor3 = Color3.fromRGB(255, 255, 255)
CommandList.BackgroundTransparency = 1
CommandList.BorderColor3 = Color3.fromRGB(0, 0, 0)
CommandList.BorderSizePixel = 0
CommandList.Position = UDim2.new(0.5, 0, 0.2, 0)
CommandList.Size = UDim2.new(1, -10, 0.8, 0)
CommandList.AutomaticCanvasSize = Enum.AutomaticSize.Y

local UIListLayout = Instance.new('UIListLayout')
UIListLayout.FillDirection = Enum.FillDirection.Vertical
UIListLayout.Parent = CommandList

local ExampleLabel = Instance.new("TextLabel")
ExampleLabel.Name = "ExampleLabel"
ExampleLabel.FontFace = Font.new("rbxasset://fonts/families/SourceSansPro.json")
ExampleLabel.Text = "command [arg1] [arg2]"
ExampleLabel.TextColor3 = Color3.fromRGB(204, 204, 204)
ExampleLabel.TextSize = 14
ExampleLabel.BackgroundColor3 = Color3.fromRGB(255, 255, 255)
ExampleLabel.BackgroundTransparency = 1
ExampleLabel.BorderColor3 = Color3.fromRGB(0, 0, 0)
ExampleLabel.BorderSizePixel = 0
ExampleLabel.Size = UDim2.new(1, 0, 0, 20)
ExampleLabel.Parent = CommandList

CommandList.Parent = CommandFrame

local TextLabel = Instance.new("TextLabel")
TextLabel.Name = "TextLabel"
TextLabel.FontFace = Font.new("rbxasset://fonts/families/RobotoMono.json")
TextLabel.Text = "Run \"uitoggle\" to hide this UI."
TextLabel.TextColor3 = Color3.fromRGB(255, 255, 255)
TextLabel.TextScaled = true
TextLabel.TextSize = 14
TextLabel.TextWrapped = true
TextLabel.AnchorPoint = Vector2.new(0.5, 0)
TextLabel.BackgroundColor3 = Color3.fromRGB(255, 255, 255)
TextLabel.BackgroundTransparency = 1
TextLabel.BorderColor3 = Color3.fromRGB(0, 0, 0)
TextLabel.BorderSizePixel = 0
TextLabel.Position = UDim2.new(0.5, 0, 0, 0)
TextLabel.Size = UDim2.new(1, -10, 0.2, 0)

local UITextSizeConstraint1 = Instance.new("UITextSizeConstraint")
UITextSizeConstraint1.Name = "UITextSizeConstraint"
UITextSizeConstraint1.MaxTextSize = 18
UITextSizeConstraint1.Parent = TextLabel

TextLabel.Parent = CommandFrame
CommandFrame.Parent = ScreenGui
ScreenGui.Parent = scriptmode and script:FindFirstAncestorWhichIsA('PlayerGui') or game:GetService('CoreGui')

local TrackingContainer = Instance.new('Folder') 
TrackingContainer.Name = 'TrackingContainer'
TrackingContainer.Parent = ScreenGui.Parent

local esp = { enabled = false, content = {} }
local rbxclass = game.IsA
local highlight

local function newtween(object, target, time)
	return game:GetService('TweenService'):Create(object, TweenInfo.new(time, Enum.EasingStyle.Sine, Enum.EasingDirection.Out), target)
end

caesar = {
	lbringtargets = {},
	instances = {},
	connections = {},

	--- @param command string
	runcommand = function(command)
		local args = split(command, ' ')
		local cmdname = args[1]
		local cmdargs = {}
		for i = 2, #args do
			table.insert(cmdargs, args[i])
		end
		local cmd = caesar.findcmd(cmdname)
		if type(cmd) == 'table' then
			pcall(cmd.callback, unpack(cmdargs))
		else
			caesar.sendnotification({ Icon = 'rbxassetid://13517960032', Title = 'caesar', Text = 'Unknown command: ' .. cmdname, Duration = 4 })
		end
	end,

	sendnotification = function(info)
		return startergui:SetCore('SendNotification', info)
	end,

	--- inspired feature by infinite yield coming soon
	customalias = {},

	findcmd = function(name)
		for _, v in pairs(caesar.commands) do
			if v.name:lower() == name:lower() or find(v.aliases, name:lower()) then
				return v
			end
		end
		return caesar.customalias[name:lower()]
	end,

	getcharacter = function(player)
		local character = select(2, pcall(function()
			return (typeof(player) == 'Instance' and player or localplayer).Character
		end))
		return typeof(character) == 'Instance' and character
	end,

	gethumanoid = function(player)
		local humanoid = select(2, pcall(function()
			return caesar.getcharacter(player):FindFirstChildWhichIsA('Humanoid')
		end))
		return typeof(humanoid) == 'Instance' and humanoid
	end,

	getrootpart = function(player)
		local rootpart = select(2, pcall(function()
			return caesar.gethumanoid(player).RootPart
		end))
		return typeof(rootpart) == 'Instance' and  rootpart
	end,

	getanimator = function(player)
		local animator = select(2, pcall(function()
			return caesar.gethumanoid(player):FindFirstChildWhichIsA('Animator')
		end))
		return typeof(animator) == 'Instance' and  animator
	end,

	ondeath = function()
		local rootpart = caesar.getrootpart()
		if typeof(rootpart) == 'Instance' and rootpart:IsA('BasePart') then
			caesar.lastposition = rootpart.Position
		end
	end,

	mouse = localplayer:GetMouse(),

	flight = {
		flying = false,
		flykeydown = nil,
		flykeyup = nil,
		flyspeed = 1,

		disconnectflightcontrols = function(self)
			if typeof(self.flykeydown) == 'RBXScriptConnection' then
				self.flykeydown:Disconnect()
			end
			if typeof(self.flykeyup) == 'RBXScriptConnection' then
				self.flykeyup:Disconnect()
			end
			if typeof(self.bodygyro) == 'Instance' then
				self.bodygyro:Destroy()
			end
			if typeof(self.bodyvelocity) == 'Instance' then
				self.bodyvelocity:Destroy()
			end
			self.flying = false
		end,

		enable = function(self)
			self:disconnectflightcontrols()

			local rootpart = caesar.getrootpart()
			local CONTROL = {F = 0, B = 0, L = 0, R = 0, Q = 0, E = 0}
			local lCONTROL = {F = 0, B = 0, L = 0, R = 0, Q = 0, E = 0}
			local SPEED = 0

			local function FLY()
				if typeof(rootpart) == 'Instance' and rootpart:IsA('BasePart') then
					self.flying = true
					local BG = Instance.new('BodyGyro')
					local BV = Instance.new('BodyVelocity')
					self.bodygryo = BG
					self.bodyvelocity = BV
					BG.P = 9e4
					BG.Parent = rootpart
					BV.Parent = rootpart
					BG.MaxTorque = Vector3.new(9e9, 9e9, 9e9)
					BG.CFrame = rootpart.CFrame
					BV.Velocity = Vector3.new(0, 0, 0)
					BV.MaxForce = Vector3.new(9e9, 9e9, 9e9)
					spawn(function()
						repeat wait()
							local humanoid = caesar.gethumanoid()
							if typeof(humanoid) == 'Instance' and humanoid:IsA('Humanoid') and not humanoid.SeatPart then
								humanoid.PlatformStand = true
							end
							if CONTROL.L + CONTROL.R ~= 0 or CONTROL.F + CONTROL.B ~= 0 or CONTROL.Q + CONTROL.E ~= 0 then
								SPEED = 50
							elseif not (CONTROL.L + CONTROL.R ~= 0 or CONTROL.F + CONTROL.B ~= 0 or CONTROL.Q + CONTROL.E ~= 0) and SPEED ~= 0 then
								SPEED = 0
							end
							if (CONTROL.L + CONTROL.R) ~= 0 or (CONTROL.F + CONTROL.B) ~= 0 or (CONTROL.Q + CONTROL.E) ~= 0 then
								BV.Velocity = ((workspace.CurrentCamera.CFrame.LookVector * (CONTROL.F + CONTROL.B)) + ((workspace.CurrentCamera.CoordinateFrame * CFrame.new(CONTROL.L + CONTROL.R, (CONTROL.F + CONTROL.B + CONTROL.Q + CONTROL.E) * 0.2, 0).Position) - workspace.CurrentCamera.CFrame.Position)) * SPEED
								lCONTROL = {F = CONTROL.F, B = CONTROL.B, L = CONTROL.L, R = CONTROL.R}
							elseif (CONTROL.L + CONTROL.R) == 0 and (CONTROL.F + CONTROL.B) == 0 and (CONTROL.Q + CONTROL.E) == 0 and SPEED ~= 0 then
								BV.Velocity = ((workspace.CurrentCamera.CFrame.LookVector * (lCONTROL.F + lCONTROL.B)) + ((workspace.CurrentCamera.CoordinateFrame * CFrame.new(lCONTROL.L + lCONTROL.R, (lCONTROL.F + lCONTROL.B + CONTROL.Q + CONTROL.E) * 0.2, 0).Position) - workspace.CurrentCamera.CFrame.Position)) * SPEED
							else
								BV.Velocity = Vector3.new(0, 0, 0)
							end
							BG.CFrame = workspace.CurrentCamera.CFrame
						until not self.flying
						CONTROL = {F = 0, B = 0, L = 0, R = 0, Q = 0, E = 0}
						lCONTROL = {F = 0, B = 0, L = 0, R = 0, Q = 0, E = 0}
						SPEED = 0
						BG:Destroy()
						BV:Destroy()
						local humanoid = caesar.gethumanoid()
						if typeof(humanoid) == 'Instance' and humanoid:IsA('Humanoid') then
							humanoid.PlatformStand = false
						end
					end)
				end
			end

			self.flykeydown = caesar.mouse.KeyDown:Connect(function(KEY)
				if KEY:lower() == 'w' then
					CONTROL.F = self.flyspeed
				elseif KEY:lower() == 's' then
					CONTROL.B = -self.flyspeed
				elseif KEY:lower() == 'a' then
					CONTROL.L = -self.flyspeed
				elseif KEY:lower() == 'd' then
					CONTROL.R = self.flyspeed
				elseif KEY:lower() == 'e' then
					CONTROL.Q = self.flyspeed * 2
				elseif KEY:lower() == 'q' then
					CONTROL.E = -self.flyspeed * 2
				end
				pcall(function() workspace.CurrentCamera.CameraType = Enum.CameraType.Track end)
			end)

			self.flykeyup = caesar.mouse.KeyUp:Connect(function(KEY)
				if KEY:lower() == 'w' then
					CONTROL.F = 0
				elseif KEY:lower() == 's' then
					CONTROL.B = 0
				elseif KEY:lower() == 'a' then
					CONTROL.L = 0
				elseif KEY:lower() == 'd' then
					CONTROL.R = 0
				elseif KEY:lower() == 'e' then
					CONTROL.Q = 0
				elseif KEY:lower() == 'q' then
					CONTROL.E = 0
				end
			end)

			FLY()
		end,

		disable = function(self)
			self:disconnectflightcontrols()
			local humanoid = caesar.gethumanoid()
			if typeof(humanoid) == 'Instance' and humanoid:IsA('Humanoid') then
				humanoid.PlatformStand = false
			end
			pcall(function()
				workspace.CurrentCamera.CameraType = Enum.CameraType.Custom
			end)
		end
	},

	getplayers = function()
		return players:GetPlayers()
	end,

	getenemies = function()
		local enemies = {}

		for _, player in pairs(players:GetPlayers()) do
			if (player.Team ~= localplayer.Team or player.Neutral) then
				insert(enemies, player)
			end
		end

		return enemies
	end,

	charadded = function(character)
		local humanoid = caesar.gethumanoid() or character:WaitForChild('Humanoid')
		if typeof(humanoid) == 'Instance' and humanoid:IsA('Humanoid') then
			insert(caesar.connections, humanoid.Died:Connect(function()
				local rootpart = caesar.getrootpart()
				if typeof(rootpart) == 'Instance' and rootpart:IsA('BasePart') then
					caesar.lastposition = rootpart.Position
				end
			end))
			local rootpart
			while typeof(rootpart) ~= 'Instance' do
				rootpart = caesar.getrootpart()
				if typeof(character) ~= 'Instance' then
					return
				end
				wait()
			end
			for _, tracer in pairs(caesar.tracers) do
				tracer:updateroot(rootpart)
			end
			local animator = caesar.getanimator()
			while typeof(animator) ~= 'Instance' do
				animator = caesar.getanimator()
				if typeof(character) ~= 'Instance' then
					return
				end
				wait()
			end
			local t = caesar.temp
			if type(t) == 'table' and t.banging then
				pcall(function() t.bangtrack:Destroy() end)
				t.bangtrack = animator:LoadAnimation(t.banganim)
				t.bangtrack:Play(0.1, 1, 1)
				t.bangtrack:AdjustSpeed(t.bangspeed)
				if typeof(t.bangdeath) == 'RBXScriptConnection' then
					t.bangdeath:Disconnect()
				end
			end
		end
	end,

	isdead = function(player)
		return select(2, pcall(function(...)
			return caesar.gethumanoid(player).Health <= 0
		end)) == true
	end,

	getdriveseat = function()
		local driveseat = select(2, pcall(function()
			return caesar.gethumanoid().SeatPart
		end))
		return typeof(driveseat) == 'Instance' and driveseat:IsA('VehicleSeat') and driveseat
	end,

	esp = {

	},

	r15 = function(player)
		return select(2, pcall(function()
			return caesar.gethumanoid(player).RigType == Enum.HumanoidRigType.R15
		end)) == true
	end,

	ac6f = { speed = 20 },
	tracers = {},
	tracer = {
		new = function(target)
			local tracer = {}
			local humanoid
			local beam = Instance.new("SelectionPartLasso")
			beam.Parent = TrackingContainer
			beam.Part = caesar.getrootpart()
			beam.Color3 = target.TeamColor.Color
			local hicharacter

			hicharacter = stepped:connect(function()
				if typeof(target) == 'Instance' and target:IsA('Player') then
					local humanoid = caesar.gethumanoid(target)
					if typeof(humanoid) == 'Instance' then
						beam.Humanoid = humanoid
					end
				else
					hicharacter:Disconnect()
				end
			end)

			insert(caesar.connections, hicharacter)

			function tracer:remove()
				print('removing tracer')
				if typeof(hicharacter) == 'RBXScriptConnection' then
					hicharacter:Disconnect()
					hicharacter = nil
					print('disconnected hicharacter')
				end
				beam:Destroy()
				print('beam destroyed')
			end

			function tracer:updateroot(newroot)
				beam.Part = newroot
			end

			print('created tracer')
			insert(caesar.tracers, tracer)
			local byebyeplayer; byebyeplayer = players.PlayerRemoving:Connect(function(player)
				if player == target then
					tracer:remove()
					if typeof(byebyeplayer) == 'RBXScriptConnection' then
						byebyeplayer:Disconnect()
					end
				end
			end)
			insert(caesar.connections, byebyeplayer)

			return tracer
		end,
	},

	findmatchinguser = function(partialName)
		local matchingUsers = {}
		if partialName == 'random' then
			local plrs = players:GetPlayers()
			table.remove(plrs, table.find(plrs, localplayer))
			return {plrs[math.random(1, #plrs)]}
		elseif partialName == 'enemy' then
			local plrs = caesar.getenemies()
			return {plrs[math.random(1, #plrs)]}
		end
		for _, ply in pairs(players:GetPlayers()) do
			if string.find(ply.Name:lower(), partialName:lower()) then
				table.insert(matchingUsers, ply)
			end
		end
		return matchingUsers
	end,

	gettargets = function(arg)
		local targets = {}

		if arg:find(',') then
			for _, v in pairs(arg:split(',')) do
				for _, plr in pairs(caesar.findmatchinguser(v)) do
					table.insert(targets, plr)
				end
			end
		else
			if arg == 'all' then
				targets = players:GetPlayers()
				table.remove(targets, table.find(targets, localplayer))
				return targets
			elseif arg == 'enemies' then
				return caesar.getenemies()
			end
			local player = caesar.findmatchinguser(arg)
			if typeof(player) == 'Instance' then
				insert(targets, player)
			end
		end

		return targets
	end,

	getplayerfromname = function(name)
		local rawname = name:lower()
		local selected

		if rawname == 'random' then
			local plrs = caesar.getplayers()
			selected = plrs[math.random(1, #plrs)]
		elseif rawname == 'enemy' then
			local plrs = caesar.getenemies()
			selected = plrs[math.random(1, #plrs)]
		else
			for _, player in ipairs(players:GetPlayers()) do
				if player.DisplayName:lower():find(rawname, 1, true) or player.Name:lower():find(rawname, 1, true) then
					selected = player
					break
				end
			end
		end

		return selected
	end,

	removecommand = function(name)
		for i, v in pairs(caesar.commands) do
			if type(v) == 'table' and v.name == name then
				table.remove(caesar, i)
			end
		end
	end,

	commands = {
		{
			name = 'uitoggle',
			aliases = {'uitog', 'togui', 'toggleui'},
			callback = function()
				CommandFrame.Visible = not CommandFrame.Visible
			end,
		},
		{
			name = 'loopbring',
			aliases = {},
			example = 'loopbring [name(s)]',
			callback = function(list)
				caesar.lbringtargets = caesar.gettargets(list)
			end,
		},
		{
			name = 'unloopbring',
			aliases = {},
			callback = function()
				caesar.lbringtargets = {}
			end,
		},
		{
			name = 'loopnotouch',
			aliases = {},
			callback = function()
				caesar.temp.loopnotouch = stepped:connect(function()
					for _, player in pairs(players:GetPlayers()) do
						if player ~= localplayer then
							caesar.disablehandletouch(player)
						end
					end
				end)
			end,
		},
		{
			name = 'unloopnotouch',
			aliases = {},
			callback = function()
				if typeof(caesar.temp.loopnotouch) == 'RBXScriptConnection' then
					caesar.temp.loopnotouch:Disconnect()
				end
			end,
		},
		{
			name = 'noclip',
			aliases = {},
			callback = function()
				caesar.temp.noclipping = stepped:connect(function()
					local character = localplayer.Character
					if typeof(character) == 'Instance' then
						for _, part in pairs(character:GetChildren()) do
							if part:IsA('BasePart') then
								part.CanCollide = false
							end
						end
					end
				end)
				caesar.sendnotification({ Title = 'caesar', Text = 'enabled noclip', Duration = 4, Button1 = 'ok' })
			end,
		},
		{
			name = 'clip',
			aliases = {},
			callback = function()
				local connection = caesar.temp.noclipping
				if typeof(connection) == 'RBXScriptConnection' then
					connection:Disconnect()
				end
				caesar.sendnotification({ Title = 'caesar', Text = 'bye bye noclip', Duration = 4, Button1 = 'ok' })
			end,
		},
		{
			name = 'fly',
			aliases = {'flight'},
			callback = function()
				caesar.flight:enable()
			end,
		},
		{
			name = 'unfly',
			aliases = {'unflight'},
			callback = function()
				caesar.flight:disable()
			end,
		},
		{
			name = 'flyspeed',
			aliases = {'flightspeed'},
			example = 'flyspeed [number]',
			callback = function(flightspeed)
				local n = tonumber(flightspeed)
				if type(n) == 'number' then
					caesar.flight.flyspeed = n
				else
					caesar.sendnotification({ Icon = 'rbxassetid://13517960032', Title = 'caesar', Text = 'flyspeed must be above 0', Duration = 4 })
				end
			end
		},
		{
			name = 'goto',
			aliases = {'to'},
			example = 'goto [name]',
			callback = function(name)
				local humanoid = caesar.gethumanoid()
				if typeof(humanoid) == 'Instance' and humanoid:IsA('Humanoid') and humanoid.Health > 0 then
					local selected = caesar.getplayerfromname(name)

					if typeof(selected) == 'Instance' and selected:IsA('Player') then
						local lroot = caesar.getrootpart()
						local troot = caesar.getrootpart(selected)

						if typeof(lroot) == 'Instance' and typeof(troot) == 'Instance' and lroot:IsA('BasePart') and troot:IsA('BasePart') then
							caesar.lastposition = lroot.Position
							lroot.AssemblyLinearVelocity = Vector3.zero
							lroot.AssemblyAngularVelocity = Vector3.zero
							lroot.CFrame = CFrame.new(troot.Position) * CFrame.new(0, 0, 2)
						end
					end
				end
			end,
		},
		{
			name = 'tppos',
			aliases = {'tpposition'},
			example = 'tppos [x] [y] [z]',
			callback = function(x, y, z)
				local x, y, z = tonumber(x), tonumber(y), tonumber(z)
				local rootpart = caesar.getrootpart()
				if typeof(rootpart) == 'Instance' and rootpart:IsA('BasePart') then
					rootpart.CFrame = CFrame.new(Vector3.new(x, y, z))
				end
			end,
		},
		{
			name = 'bang',
			aliases = {},
			example = 'bang [name]',
			callback = function(name, speed)
				caesar.runcommand('unbang')

				local speed = tonumber(speed)
				local selected = caesar.getplayerfromname(name)
				local humanoid = caesar.gethumanoid()
				local animator = caesar.getanimator()
				local t = caesar.temp

				--- luals annotation [vvv]
				--- @cast humanoid Humanoid
				--- @cast animator Animator
				if typeof(selected) == 'Instance' and selected:IsA('Player') and typeof(humanoid) == 'Instance' and typeof(animator) == 'Instance' then
					t.banganim = Instance.new('Animation')
					t.banganim.AnimationId = not caesar.r15() and "rbxassetid://148840371" or "rbxassetid://5918726674"
					t.bangtrack = animator:LoadAnimation(t.banganim)
					t.bangtrack:Play(0.1, 1, 1)
					t.bangspeed = speed or 3
					t.bangtrack:AdjustSpeed(t.bangspeed)
					t.bangdeath = humanoid.Died:Connect(function()
						t.bangtrack:Stop()
						t.banganim:Destroy()
						t.bangdeath:Disconnect()
					end)
					t.banging = stepped:connect(function()
						local proot = caesar.getrootpart()
						local troot = caesar.getrootpart(selected)

						if typeof(proot) == 'Instance' and proot:IsA('BasePart') and typeof(troot) == 'Instance' and troot:IsA('BasePart') then
							proot.CFrame = troot.CFrame * CFrame.new(0, 0, 1.1)
						end
					end)

					insert(caesar.connections, t.banging)
					insert(caesar.connections, t.bangdeath)
				end
			end,
		},
		{
			name = 'unbang',
			aliases = {},
			callback = function()
				local t = caesar.temp
				if typeof(t.banging) == 'RBXScriptConnection' then
					t.banging:Disconnect()
				end
				if typeof(t.bangdeath) == 'RBXScriptConnection' then
					t.bangdeath:Disconnect()
				end
				if typeof(t.bangtrack) == 'Instance' and t.bangtrack:IsA('AnimationTrack') then
					t.bangtrack:Stop()
				end
				if typeof(t.banganim) == 'Instance' then
					t.banganim:Destroy()
				end
			end,
		},
		{
			name = 'back',
			aliases = {'flashback'},
			callback = function()
				local humanoid = caesar.gethumanoid()
				if typeof(humanoid) == 'Instance' and humanoid:IsA('Humanoid') and humanoid.Health > 0 then
					local lroot = caesar.getrootpart()
					if typeof(lroot) == 'Instance' and lroot:IsA('BasePart') and typeof(caesar.lastposition) == 'Vector3' then
						local pos = lroot.Position
						lroot.AssemblyLinearVelocity = Vector3.zero
						lroot.AssemblyAngularVelocity = Vector3.zero
						lroot.CFrame = CFrame.new(caesar.lastposition)
						caesar.lastposition = pos
					end
				end
			end,
		},

		{
			name = 'bigheads',
			aliases = {},
			callback = function()
				caesar.temp.bigheads = stepped:connect(function()
					for _, player in pairs(players:GetPlayers()) do
						if player ~= players.LocalPlayer then
							pcall(function()
								local head = player.Character.Head
								head.CanCollide = false
								if caesar.isdead(player) or player.Team == players.LocalPlayer.Team then
									head.Size = Vector3.one
								else
									head.Size = Vector3.one * 100
								end
							end)
						end
					end
				end)
			end,
		},

		{
			name = 'unbigheads',
			aliases = {'nobigheads'},
			callback = function()
				if typeof(caesar.temp.bigheads) == 'RBXScriptConnection' then
					caesar.temp.bigheads:Disconnect()
				end
				for _, player in pairs(players:GetPlayers()) do
					if player ~= players.LocalPlayer then
						pcall(function()
							local head = player.Character.Head
							head.CanCollide = false
							head.Size = Vector3.one
						end)
					end
				end
			end,
		},

		{
			name = 'speed',
			aliases = {'ws'},
			example = 'speed [number]',
			callback = function(speed)
				if typeof(caesar.temp.loopspeeder) == 'RBXScriptConnection' then
					return caesar.sendnotification({ Icon = 'rbxassetid://13517960032', Title = 'caesar', Text = 'run \'unloopspeed\' first', Duration = 4 })
				end
				local humanoid = caesar.gethumanoid()
				if typeof(humanoid) == 'Instance' and humanoid:IsA('Humanoid') then
					local n = tonumber(speed)
					if type(n) == 'number' and n > 0 then
						humanoid.WalkSpeed = n
					end
				end
			end,
		},
		{
			name = 'loopspeed',
			aliases = {'loopws'},
			callback = function(speed)
				caesar.runcommand('unloopspeed')
				local humanoid = caesar.gethumanoid()
				if typeof(humanoid) == 'Instance' and humanoid:IsA('Humanoid') then
					local n = tonumber(speed)
					if type(n) == 'number' and n > 0 then
						caesar.temp.loopspeeder = stepped:connect(function()
							humanoid.WalkSpeed = n
						end)
					end
				end
			end,
		},
		{
			name = 'unloopspeed',
			aliases = {'unloopws'},
			callback = function()
				if typeof(caesar.temp.loopspeeder) == 'RBXScriptConnection' then
					caesar.temp.loopspeeder:Disconnect()
					caesar.temp.loopspeeder = nil
				end
			end,
		},
		{
			name = 'fling',
			aliases = {},
			--- This function is a partial tweak of an Infinite Yield function.
			callback = function()
				caesar.flinging = false
				local character = localplayer.Character
				local humanoid = caesar.gethumanoid()
				if typeof(character) == 'Instance' and character:IsA('Model') and typeof(humanoid) == 'Instance' and humanoid:IsA('Humanoid') and humanoid.Health > 0 then
					for _, child in pairs(character:GetDescendants()) do
						if child:IsA("BasePart") then
							child.CustomPhysicalProperties = PhysicalProperties.new(math.huge, 0.3, 0.5)
						end
					end
					caesar.runcommand('noclip')
					wait(.1)
					local bambam = Instance.new("BodyAngularVelocity")
					bambam.Name = tostring(math.random()):gsub('.', '')
					bambam.Parent = caesar.getrootpart()
					bambam.AngularVelocity = Vector3.new(0, 99999, 0)
					bambam.MaxTorque = Vector3.new(0, math.huge, 0)
					bambam.P = math.huge
					for _, part in pairs(character:GetChildren()) do
						if part:IsA('BasePart') then
							part.CanCollide = false
							part.Massless = true
							part.AssemblyLinearVelocity = Vector3.zero
						end
					end
					caesar.flinging = true
					caesar.temp.flingingdeath = humanoid.Died:Connect(function()
						caesar.runcommand('unfling')
						if typeof(caesar.temp.flingingdeath) == 'RBXScriptConnection' then
							caesar.temp.flingingdeath:Disconnect()
						end
					end)
					while caesar.flinging == true do
						bambam.AngularVelocity = Vector3.new(0,99999,0)
						wait(.2)
						bambam.AngularVelocity = Vector3.new(0,0,0)
						wait(.1)
					end
				end
			end,
		},
		{
			name = 'unfling',
			aliases = {},
			--- This function is a partial tweak of an Infinite Yield function.
			callback = function()
				caesar.runcommand('clip')
				if typeof(caesar.temp.flingingdeath) == 'RBXScriptConnection' then
					caesar.temp.flingingdeath:Disconnect()
				end
				caesar.flinging = false
				wait(.1)
				local character = localplayer.Character
				local rootpart = caesar.getrootpart()
				if typeof(character) == 'Instance' and typeof(rootpart) == 'Instance' and rootpart:IsA('BasePart') then
					for _, v in pairs(rootpart:GetChildren()) do
						if v:IsA('BodyAngularVelocity') then
							v:Destroy()
						end
					end
					for _, v in pairs(character:GetChildren()) do
						if v:IsA('BasePart') then
							v.CustomPhysicalProperties = PhysicalProperties.new(.7, .3, .5)
						end
					end
				end
			end,
		},
		{
			name = 'vspeed',
			aliases = {'ac6f'},
			example = 'vspeed (new, makes car faster)',
			callback = function()
				if typeof(caesar.temp.ac6f) == 'RBXScriptConnection' then
					caesar.temp.ac6f:Disconnect()
				end

				local w, s = Enum.KeyCode.W, Enum.KeyCode.S
				local c, t, f, d = 1, 0, 60, 1 / 60
				local applicablevelocity = Vector3.zero

				caesar.temp.ac6f = stepped:connect(function(time, dt)
					if 1 / dt > f or time > t + d then
						local driveseat = caesar.getdriveseat()
						if typeof(driveseat) == 'Instance' then
							if uis:IsKeyDown(w) then
								c = 1 + (caesar.ac6f.speed / 10) * dt
								applicablevelocity = Vector3.one * c
								driveseat.Velocity = driveseat.Velocity * applicablevelocity
							elseif uis:IsKeyDown(s) then
								applicablevelocity = Vector3.one * c
								c = 1 - (caesar.ac6f.speed / 10) * dt
								driveseat.Velocity = driveseat.Velocity * applicablevelocity
							end
						end
					end
				end)
			end,
		},
		{
			name = 'vspeedset',
			aliases = {'ac6fspeed'},
			example = 'vspeedset [multiplier]',
			callback = function(speed)
				local n = tonumber(speed)
				if type(n) == 'number' and n >= 10 and n <= 30 then 
					caesar.ac6f.speed = n
				else
					caesar.sendnotification({ Icon = 'rbxassetid://13517960032', Title = 'caesar', Text = 'vspeed must be between 10 and 30', Duration = 4 })
				end
			end,
		},
		{
			name = 'unvspeed',
			aliases = {'unac6f'},
			callback = function()
				if typeof(caesar.temp.ac6f) == 'RBXScriptConnection' then
					caesar.temp.ac6f:Disconnect()
				else
					caesar.sendnotification({ Icon = 'rbxassetid://13517960032', Title = 'caesar', Text = 'vspeed is already off', Duration = 4 })
				end
			end,
		},
		{
			name = 'destroyheight',
			aliases = {},
			example = 'destroyheight [number]',
			callback = function(height)
				workspace.FallenPartsDestroyHeight = height
			end,
		},
		{
			name = 'nodestroyheight',
			aliases = {},
			callback = function()
				workspace.FallenPartsDestroyHeight = -1e8
				caesar.sendnotification({ Icon = 'rbxassetid://15823591204', Title = 'caesar', Text = 'ok it\'ll take a while to reach the destroyheight', Duration = 10, Button1 = 'understood' })
			end,
		},
		{
			name = 'exit',
			aliases = {'quit', 'closegame'},
			example = 'exit / quit / closegame',
			callback = function()
				game:Shutdown()
			end,
		},
		{
			name = 'ambient',
			aliases = {},
			example = 'ambient [r] [g] [b] (CLIENT)',
			callback = function(r, g, b)
				local r, g, b = tonumber(r), tonumber(g), tonumber(b)
				if type(r) == 'number' and type(g) == 'number' and type(b) == 'number' then
					lighting.Ambient = Color3.fromRGB(r, g, b)
				end
			end,
		},
		{
			name = 'fullbright',
			aliases = {},
			callback = function()
				caesar.temp.fullbright = stepped:connect(function()
					lighting.Ambient = Color3.new(1, 1, 1)
					lighting.OutdoorAmbient = Color3.new(1, 1, 1)
					lighting.GlobalShadows = false
				end)
			end,
		},
		{
			name = 'unfullbright',
			aliases = {'nofullbright'},
			callback = function()
				if typeof(caesar.temp.fullbright) == 'RBXScriptConnection' then
					caesar.temp.fullbright:Disconnect()
				end
			end,
		},
		{
			name = 'fogend',
			aliases = {},
			example = 'fogend [number] (CLIENT)',
			callback = function(fogend)
				local n = tonumber(fogend)
				if type(n) == 'number' then
					game:GetService('Lighting').FogEnd = n
				end
			end,
		},
		{
			name = 'fogstart',
			aliases = {},
			example = 'fogstart [number] (CLIENT)',
			callback = function(start)
				local n = tonumber(start)
				if type(n) == 'number' then
					game:GetService('Lighting').FogStart = n
				end
			end,
		},
		{
			name = 'breakcam',
			aliases = {'breakcamera'},
			callback = function()
				localplayer.CameraMinZoomDistance = math.huge - math.huge
				localplayer.CameraMaxZoomDistance = math.huge - math.huge
			end,
		},
		{
			name = 'fixcam',
			aliases = {},
			callback = function()
				workspace.CurrentCamera:Destroy()
				wait(.1)
				local humanoid
				while typeof(humanoid) ~= 'Instance' do
					humanoid = caesar.gethumanoid()
					wait()
				end
				workspace.CurrentCamera.CameraSubject = humanoid
				workspace.CurrentCamera.CameraType = Enum.CameraType.Custom
				localplayer.CameraMinZoomDistance = 0.5
				localplayer.CameraMaxZoomDistance = 400
				localplayer.CameraMode = Enum.CameraMode.Classic
			end,
		},
		{
			name = 'enableshiftlock',
			aliases = {'allowshiftlock', 'enableslock', 'allowslock'},
			callback = function()
				localplayer.DevEnableMouseLock = true
				caesar.sendnotification({ Icon = 'rbxassetid://67790032', Title = 'caesar', Text = 'you can now modify shiftlock behavior', Duration = 10, Button1 = 'ok' })
			end,
		},
		{
			name = 'maxzoom',
			aliases = {},
			example = 'maxzoom [number]',
			callback = function(maxzoom)
				local n = tonumber(maxzoom)
				if type(n) == 'number' then
					localplayer.CameraMaxZoomDistance = n
				end
			end,
		},
		{
			name = 'view',
			aliases = {},
			example = 'view [name]',
			callback = function(name)
				caesar.runcommand('unview')
				local t = caesar.temp
				local selected = caesar.getplayerfromname(name)
				if typeof(selected) == 'Instance' and selected:IsA('Player') then
					local character = selected.Character
					if typeof(character) == 'Instance' then
						t.viewlock = selected.CharacterAdded:Connect(function(character)
							if typeof(t.viewdisconnect) == 'RBXScriptConnection' then
								workspace.CurrentCamera.CameraSubject = character
							end
						end)
						t.viewdisconnect = players.PlayerRemoving:Connect(function(player)
							if selected == player and typeof(t.viewdisconnect) == 'RBXScriptConnection' then
								workspace.CurrentCamera.CameraSubject = caesar.getcharacter()
								t.viewdisconnect:Disconnect()
								t.viewdisconnect = nil
							end
						end)
						local character = selected.Character
						if typeof(character) == 'Instance' then
							workspace.CurrentCamera.CameraSubject = character
						end
						workspace.CurrentCamera.CameraSubject = character
					end
				end
			end,
		},
		{
			name = 'unview',
			aliases = {},
			example = 'unview [name]',
			callback = function(name)
				local t = caesar.temp
				if typeof(t.viewlock) == 'RBXScriptConnection' then
					t.viewlock:Disconnect()
				end
				if typeof(t.viewdisconnect) == 'RBXScriptConnection' then
					t.viewdisconnect:Disconnect()
				end
				workspace.CurrentCamera.CameraSubject = caesar.gethumanoid()
			end,
		},
		{
			name = 'music',
			aliases = {'play'},
			example = 'music [soundid] (CLIENT)',
			callback = function(soundid)
				local sound = playergui:FindFirstChild('CaeserMusic')
				if typeof(sound) == 'Instance' and sound:IsA('Sound') then
					sound:Destroy()
				end
				sound = Instance.new('Sound')
				sound.Name = 'CaeserMusic'
				sound.Volume = 1
				sound.PlaybackSpeed = 1
				sound.SoundId = 'rbxassetid://' .. soundid
				sound.Looped = true
				sound.Parent = playergui
				sound:Play()
			end,
		},
		{
			name = 'volume',
			aliases = {'vol'},
			example = 'volume [number] (CLIENT)',
			callback = function(volume)
				local n = tonumber(volume)
				if type(n) == 'number' and n > 0 and n <= 10 then
					local sound = playergui:FindFirstChild('CaeserMusic')
					if typeof(sound) == 'Instance' and sound:IsA('Sound') then
						sound.Volume = n
					end
				end
			end,
		},
		{
			name = 'pitch',
			aliases = {'playbackspeed'},
			example = 'pitch [number] (CLIENT)',
			callback = function(pitch)
				local n = tonumber(pitch)
				if type(n) == 'number' and n > 0 and n <= 10 then
					local sound = playergui:FindFirstChild('CaeserMusic')
					if typeof(sound) == 'Instance' and sound:IsA('Sound') then
						sound.PlaybackSpeed = pitch
					end
				end
			end,
		},
		{
			name = 'stop',
			aliases = {'nomusic', 'unmusic'},
			example = 'stop (CLIENT)',
			callback = function()
				local sound = playergui:FindFirstChild('CaeserMusic')
				if typeof(sound) == 'Instance' and sound:IsA('Sound') then
					sound:Destroy()
				else
					caesar.sendnotification({ Icon = 'rbxassetid://13517960032', Title = 'caesar', Text = 'no music is playing', Duration = 4 })
				end
			end,
		},
		{
			name = 'unweaken',
			aliases = {},
			callback = function()
				local character = localplayer.Character
				if typeof(character) == 'Instance' and character:IsA('Model') then
					for _, child in pairs(character:GetDescendants()) do
						if child.ClassName == "Part" then
							child.CustomPhysicalProperties = PhysicalProperties.new(0.7, 0.3, 0.5)
						end
					end
				end
			end,
		},
		{
			name = 'tracers',
			aliases = {},
			callback = function()
				local rootpart = caesar.getrootpart()
				if typeof(rootpart) == 'Instance' and rootpart:IsA('BasePart') then
					for i, tracer in pairs(caesar.tracers) do
						tracer:remove()
						caesar.tracers[i] = nil
					end
					local function newtracer(player)
						if player ~= localplayer and (player.Team ~= localplayer.Team or player.Neutral) then
							local tracer = caesar.tracer.new(player)
							tracer:updateroot(rootpart)
						end
					end
					for _, player in pairs(players:GetPlayers()) do
						newtracer(player)
					end
					caesar.temp.newtracer = players.PlayerAdded:Connect(newtracer)
				else
					caesar.sendnotification({ Icon = 'rbxassetid://13517960032', Title = 'caesar', Text = 'you lack a rootpart', Duration = 4 })
				end
			end,
		},
		{
			name = 'untracers',
			aliases = {'notracers'},
			callback = function()
				for i, tracer in pairs(caesar.tracers) do
					tracer:remove()
					caesar.tracers[i] = nil
				end
				if typeof(caesar.temp.newtracer) == 'RBXScriptConnection' then
					caesar.temp.newtracer:Disconnect()
					caesar.temp.newtracer = nil
				end
			end,
		},
		{
			name = 'esp',
			aliases = {},
			callback = function()
				esp.enabled = true
			end,
		},
		{
			name = 'unesp',
			aliases = {},
			callback = function()
				esp.enabled = false
			end,
		}
	},

	focused = function()

	end,

	focuslost = function(e)
		if e == true then
			newtween(ExecutionFrame, {Position = UDim2.new(.5, 0, -.5, 0)}, .3):Play()
			caesar.runcommand(ExecutionField.Text)
			ExecutionField.Text = ''
		end
	end,

	hasforcefield = function(player)
		return typeof(select(2, pcall(function(...)
			return player.Character:FindFirstChildWhichIsA('ForceField')
		end))) == 'Instance'
	end,

	disablehandletouch = function(player)
		pcall(function()
			player:FindFirstChildWhichIsA('Tool').Handle.CanTouch = false
		end)
	end,

	temp = {
		lbringtargets = stepped:connect(function()
			if #caesar.lbringtargets > 0 then
				for _, plr in pairs(caesar.lbringtargets) do
					if plr ~= localplayer and not caesar.hasforcefield(plr) then
						caesar.disablehandletouch(plr)

						local proot = caesar.getrootpart()
						local troot = caesar.getrootpart(plr)

						if typeof(troot) == 'Instance' and troot:IsA('BasePart') and typeof(proot) == 'Instance' and proot:IsA('BasePart') then
							troot.CFrame = proot.CFrame + Vector3.new(2, 1, 0)
						end
					end
				end
			end
		end)
	},
	cleanup = function()
		caesar.runcommand('unbigheads')
		caesar.flight:disable()
		for _, entry in pairs(esp.content) do
			if type(entry) == 'table' then
				pcall(entry.destroy, entry)
			end
		end
		if type(caesar.instances) == 'table' then
			for _, instance in pairs(caesar.instances) do
				if typeof(instance) == 'Instance' then
					instance:Destroy()
				end
			end
		end
		if type(caesar.connections) == 'table' then
			for _, connection in pairs(caesar.connections) do
				if typeof(connection) == 'RBXScriptConnection' then
					connection:Disconnect()
				end
			end
		end
		if type(caesar.temp) == 'table' then
			for _, connection in pairs(caesar.temp) do
				if typeof(connection) == 'RBXScriptConnection' then
					connection:Disconnect()
				end
			end
		end
	end
}

if scriptmode then
	caesar.removecommand('bang')
end

-- load command list

for _, command in pairs(caesar.commands) do
	local label = ExampleLabel:Clone()
	label.Text = command.example or command.name
	label.Parent = ExampleLabel.Parent
end
ExampleLabel.Visible = false

do
	local c3u = Color3.fromRGB
	local color_scheme = {}
	color_scheme['nearest'] = c3u(0, 172, 255)
	color_scheme['valid'] = c3u(38, 255, 99)
	color_scheme['invalid'] = c3u(255, 37, 40)
	
	function esp.can_track(player, character)
		if not esp.enabled then
			return false
		end
		if typeof(player) == 'Instance' and rbxclass(player, "Model") then
			character = player
			player = players:GetPlayerFromCharacter(character)
		end
		return player and player ~= localplayer and (player.Team ~= localplayer.Team or player.Neutral)
	end

	local highlight = {}
	highlight.__index = highlight

	function highlight.new(player)
		if esp.content[player] then
			return esp.content[player]
		end
		local self = setmetatable({}, highlight)
		self.player = player
		local highlight = Instance.new("Highlight")
		highlight.Enabled = esp.can_track(player)
		highlight.FillColor = player.TeamColor.Color
		highlight.OutlineColor = player.TeamColor.Color
		highlight.Parent = TrackingContainer
		insert(caesar.connections, player.CharacterAdded:Connect(function(character)
			self.character = character
			highlight.Adornee = character
			highlight.Enabled = esp.can_track(player)
		end))
		if player.Character then
			self.character = player.Character
			highlight.Adornee = player.Character
			highlight.Enabled = esp.can_track(player)
		end
		insert(caesar.connections, player:GetPropertyChangedSignal("Team"):Connect(function()
			highlight.Enabled = esp.can_track(player)
		end))
		self.highlight = highlight
		esp.content[player] = self
		return self
	end

	function highlight:destroy()
		self.highlight:Destroy()
	end

	function highlight:valid()
		return esp.can_track(self.player)
	end

	function highlight:color(color)
		self.highlight.FillColor = color
		self.highlight.OutlineColor = color
	end

	function highlight:enabled(enabled)
		self.highlight.Enabled = enabled
	end

	local function load_player(player)
		if player ~= localplayer then
			highlight.new(player)
		end
	end

	for _, player in pairs(players:GetPlayers()) do
		load_player(player)
	end

	insert(caesar.connections, players.PlayerAdded:Connect(load_player))
	insert(caesar.connections, stepped:connect(function()
		if esp.enabled then
			for player, entry in pairs(esp.content) do
				if typeof(player) == 'Instance' and player:IsA('Player') and type(entry) == 'table' then
					entry:enabled(esp.can_track(entry.player))
					entry:color(player.TeamColor.Color)
				elseif type(entry) == 'table' then
					entry:destroy()
				end
			end
		else
			for _, entry in pairs(esp.content) do
				if type(entry) == 'table' then
					entry:enabled(false)
				end
			end
		end
	end))
end

local character = localplayer.Character
if typeof(character) == 'Instance' and character:IsA('Model') then
	caesar.charadded(character)
end

insert(caesar.connections, localplayer.CharacterAdded:Connect(caesar.charadded))
insert(caesar.connections, ExecutionField.Focused:Connect(caesar.focused))
insert(caesar.connections, ExecutionField.FocusLost:Connect(caesar.focuslost))
insert(caesar.connections, uis.InputBegan:Connect(function(io, gpe)
	if gpe == true then
		return
	end

	if io.KeyCode.Name == 'Insert' then
		newtween(ExecutionFrame, {Position = UDim2.new(.5, 0, 0, 30)}, .1):Play()
		wait()
		ExecutionField:CaptureFocus()
	end
end))
insert(caesar.instances, TrackingContainer)
insert(caesar.instances, ScreenGui)
caesar.sendnotification({ Icon = 'rbxassetid://17365669760', Title = 'caesar', Text = 'caesar has loaded, enjoy\n- byteveil', Duration = 10, Button1 = 'thank u' })
caesar.sendnotification({ Icon = 'rbxassetid://17365761837', Title = 'caesar', Text = 'press the quote key to use the command bar', Duration = 10, Button1 = 'understood' })
_G.caesar = caesar