CREATE TABLE `check_scanner`.`itemsStatus` (
  `ItemID` INT(11) NOT NULL COMMENT '',
  `Exported` TinyInt(1) NOT NULL DEFAULT 0 COMMENT 'Indicates if this item has been exported by the Crossroads Check Batch Processor tool',
  `ErrorMessage` Varchar(2048) COMMENT 'An error message (if any) for the last export attempt',
  PRIMARY KEY (`ItemID`)  COMMENT '',
  CONSTRAINT `FK_ItemsStatus_ItemID`
    FOREIGN KEY (`ItemID`)
    REFERENCES `check_scanner`.`items` (`ID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION);
