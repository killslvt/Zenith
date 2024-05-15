local userInputService = game:GetService("UserInputService")

local player = game.Players.localPlayer
local character = player.Character or player.CharacterAdded:Wait()
local humanoid = character:WaitForChild("Humanoid")

local function InfJump(input, gameProcess)
    if gameProcess then return end
    if input.KeyCode == Enum.KeyCode.Space then
        humanoid:ChangeState(Enum.HumanoidStateType.Jumping)
    end
end

userInputService.InputBegan:Connect(InfJump)