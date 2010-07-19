<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:p="clr-namespace:TheBoxSoftware.DeveloperSuite.LiveDocumentor.Pages;assembly=Live Documenter" 
    xmlns:bbdsldpe="clr-namespace:TheBoxSoftware.DeveloperSuite.LiveDocumentor.Pages.Elements;assembly=Live Documenter" 
    xmlns:av="http://schemas.microsoft.com/winfx/2006/xaml/presentation" 
    xmlns:bbdsld="clr-namespace:TheBoxSoftware.DeveloperSuite.LiveDocumentor;assembly=Live Documenter" 
    xmlns:s="clr-namespace:System;assembly=mscorlib"
    exclude-result-prefixes="msxsl bbdsldpe av bbdsld s p"
    >
  <xsl:output method="xml" indent="yes"/>
  
  <!-- throw away content for these -->
  <xsl:template match="av:Style">
  </xsl:template>
  <!-- end throw away content -->

  <xsl:template match="/">
    <livedocument>
      <xsl:apply-templates />
    </livedocument>
  </xsl:template>
  
  <xsl:template match="bbdsldpe:SummaryTable">
    <table>
      <xsl:for-each select="av:TableRowGroup/av:TableRow">
        <tablerow>
          <xsl:for-each select="av:TableCell">
            <tablecell><xsl:apply-templates /></tablecell>
          </xsl:for-each>
        </tablerow>
      </xsl:for-each>
    </table>
  </xsl:template>

  <xsl:template match="bbdsldpe:Header1">
    <header1><xsl:apply-templates /></header1>
  </xsl:template>

  <xsl:template match="bbdsldpe:Header2">
    <header2><xsl:apply-templates /></header2>
  </xsl:template>

  <xsl:template match="av:Paragraph">
    <paragraph>
      <xsl:apply-templates />
    </paragraph>
  </xsl:template>

  <xsl:template match="av:Bold">
    <bold><xsl:value-of select="."/></bold>
  </xsl:template>

  <xsl:template match="av:Hyperlink">
    <hyperlink>
      <xsl:attribute name="uri">
        <xsl:value-of select="av:Hyperlink.Tag" />
      </xsl:attribute>
      <xsl:apply-templates />
    </hyperlink>
  </xsl:template>

  <xsl:template match="bbdsldpe:Code" xml:space="preserve">
    <code><xsl:apply-templates /></code>
  </xsl:template>

  <xsl:template match="bbdsldpe:Keyword" xml:space="preserve">
    <keyword><xsl:apply-templates /></keyword>
  </xsl:template>

  <xsl:template match="av:Run">
    <run><xsl:apply-templates /></run>
  </xsl:template>

  <xsl:template match="bbdsldpe:Summary">
    <summary>
      <xsl:apply-templates />
    </summary>
  </xsl:template>

  <xsl:template match="av:List">
    <list>
      <xsl:for-each select="av:ListItem">
        <item><xsl:apply-templates /></item>
      </xsl:for-each>
    </list>
  </xsl:template>
</xsl:stylesheet>


