
--
-- Indexes for dumped tables
--

--
-- Indexes for table `app_themes`
--
ALTER TABLE `app_themes`
  ADD PRIMARY KEY (`ThemeName`);

--
-- Indexes for table `debug_matching`
--
ALTER TABLE `debug_matching`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `inv_inventory`
--
ALTER TABLE `inv_inventory`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `idx_partid_location` (`PartID`,`Location`),
  ADD KEY `idx_operation` (`Operation`),
  ADD KEY `idx_receivedate` (`ReceiveDate`);

--
-- Indexes for table `inv_transaction`
--
ALTER TABLE `inv_transaction`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `idx_partid` (`PartID`),
  ADD KEY `idx_user` (`User`),
  ADD KEY `idx_datetime` (`ReceiveDate`);

--
-- Indexes for table `log_changelog`
--
ALTER TABLE `log_changelog`
  ADD PRIMARY KEY (`Version`);

--
-- Indexes for table `log_error`
--
ALTER TABLE `log_error`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `idx_errortime` (`ErrorTime`),
  ADD KEY `idx_user` (`User`),
  ADD KEY `idx_severity` (`Severity`),
  ADD KEY `idx_errortype` (`ErrorType`);

--
-- Indexes for table `md_item_types`
--
ALTER TABLE `md_item_types`
  ADD PRIMARY KEY (`ID`),
  ADD UNIQUE KEY `uq_type` (`ItemType`);

--
-- Indexes for table `md_locations`
--
ALTER TABLE `md_locations`
  ADD PRIMARY KEY (`ID`),
  ADD UNIQUE KEY `uq_location` (`Location`);

--
-- Indexes for table `md_operation_numbers`
--
ALTER TABLE `md_operation_numbers`
  ADD PRIMARY KEY (`ID`),
  ADD UNIQUE KEY `uq_operation` (`Operation`);

--
-- Indexes for table `md_part_ids`
--
ALTER TABLE `md_part_ids`
  ADD PRIMARY KEY (`ID`),
  ADD UNIQUE KEY `uq_item_number` (`PartID`);

--
-- Indexes for table `migration_debug_log`
--
ALTER TABLE `migration_debug_log`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `sys_last_10_transactions`
--
ALTER TABLE `sys_last_10_transactions`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `idx_user_datetime` (`User`,`ReceiveDate`);

--
-- Indexes for table `sys_roles`
--
ALTER TABLE `sys_roles`
  ADD PRIMARY KEY (`ID`),
  ADD UNIQUE KEY `uq_rolename` (`RoleName`);

--
-- Indexes for table `sys_user_roles`
--
ALTER TABLE `sys_user_roles`
  ADD PRIMARY KEY (`UserID`,`RoleID`),
  ADD KEY `idx_userid` (`UserID`),
  ADD KEY `idx_roleid` (`RoleID`);

--
-- Indexes for table `usr_ui_settings`
--
ALTER TABLE `usr_ui_settings`
  ADD PRIMARY KEY (`UserId`);

--
-- Indexes for table `usr_users`
--
ALTER TABLE `usr_users`
  ADD PRIMARY KEY (`ID`),
  ADD UNIQUE KEY `uq_user` (`User`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `debug_matching`
--
ALTER TABLE `debug_matching`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `inv_inventory`
--
ALTER TABLE `inv_inventory`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=44697;

--
-- AUTO_INCREMENT for table `inv_transaction`
--
ALTER TABLE `inv_transaction`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=18259;

--
-- AUTO_INCREMENT for table `log_error`
--
ALTER TABLE `log_error`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=17;

--
-- AUTO_INCREMENT for table `md_item_types`
--
ALTER TABLE `md_item_types`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT for table `md_locations`
--
ALTER TABLE `md_locations`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=55370;

--
-- AUTO_INCREMENT for table `md_operation_numbers`
--
ALTER TABLE `md_operation_numbers`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=151;

--
-- AUTO_INCREMENT for table `md_part_ids`
--
ALTER TABLE `md_part_ids`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5095;

--
-- AUTO_INCREMENT for table `migration_debug_log`
--
ALTER TABLE `migration_debug_log`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT for table `sys_last_10_transactions`
--
ALTER TABLE `sys_last_10_transactions`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=731;

--
-- AUTO_INCREMENT for table `sys_roles`
--
ALTER TABLE `sys_roles`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT for table `usr_users`
--
ALTER TABLE `usr_users`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=311;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `sys_user_roles`
--
ALTER TABLE `sys_user_roles`
  ADD CONSTRAINT `sys_user_roles_ibfk_1` FOREIGN KEY (`UserID`) REFERENCES `usr_users` (`ID`) ON DELETE CASCADE,
  ADD CONSTRAINT `sys_user_roles_ibfk_2` FOREIGN KEY (`RoleID`) REFERENCES `sys_roles` (`ID`) ON DELETE CASCADE;

--
-- Constraints for table `usr_ui_settings`
--
ALTER TABLE `usr_ui_settings`
  ADD CONSTRAINT `usr_ui_settings_ibfk_1` FOREIGN KEY (`UserId`) REFERENCES `usr_users` (`User`);
