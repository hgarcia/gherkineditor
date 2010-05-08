namespace :generate do
  desc "Generating Gherkin editor syntax filefor all i18n supported languages in Gherkin"

	task :hlight do
		require 'gherkin/i18n'
		require 'erb'		
		puts "Building the highlights"
		xmlfile = ERB.new(IO.read(File.dirname(__FILE__) + '/customHighlighting.xshd.erb'), nil, '-')
		File.open('../GherkinEditor/CustomHighlighting.xshd', 'wb') do |io|
			keywords = Gherkin::I18n.keyword_regexp(:feature,:background)
			scenarios = Gherkin::I18n.keyword_regexp(:scenario,:scenario_outline)
			steps = Gherkin::I18n.keyword_regexp(:step,:examples)
			languages = Gherkin::I18n.languages()
			io.write(xmlfile.result(binding))
		end
	end

	task :h do
		require 'gherkin/i18n'
		require 'erb'
		
		i18n = Gherkin::I18n.new("es")
		puts i18n.keywords(:examples)[0]
	end

	task :codecomp do
		require 'gherkin/i18n'
		require 'erb'	
		puts "Building the code completion"	
		xmlfile = ERB.new(IO.read(File.dirname(__FILE__) + '/CompletionDataLoader.cs.erb'), nil, '-')
		File.open('../GherkinEditor/CompletionDataLoader.cs', 'wb') do |io|
			langs = Gherkin::I18n.all
			io.write(xmlfile.result(binding))
		end
	end

	task :langs do
		require 'gherkin/i18n'
		require 'erb'	
		puts "Building the languages"	
		xmlfile = ERB.new(IO.read(File.dirname(__FILE__) + '/Languages.cs.erb'), nil, '-')
		File.open('../GherkinEditor/Languages.cs', 'wb') do |io|
			langs = Gherkin::I18n.all
			
			io.write(xmlfile.result(binding))
		end
	end

end
